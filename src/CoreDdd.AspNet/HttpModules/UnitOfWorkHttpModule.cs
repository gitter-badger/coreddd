﻿using System;
using System.Data;
using System.Web;
using CoreDdd.Domain.Events;
using CoreDdd.UnitOfWorks;

namespace CoreDdd.AspNet.HttpModules
{
    public class UnitOfWorkHttpModule : IHttpModule
    {
        private const string UnitOfWorkSessionKey = "CoreDdd_UnitOfWorkHttpModule_UnitOfWork";

        private static IUnitOfWorkFactory _unitOfWorkFactory;
        private static IsolationLevel _isolationLevel;

        public void Init(HttpApplication application)
        {
            application.BeginRequest += Application_BeginRequest;
            application.EndRequest += Application_EndRequest;
            application.Error += Application_Error;
        }

        public static void Initialize(
            IUnitOfWorkFactory unitOfWorkFactory,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _isolationLevel = isolationLevel;
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            _CheckWasInitialized();

            UnitOfWork = _unitOfWorkFactory.Create();
            UnitOfWork.BeginTransaction(_isolationLevel);            
        }

        private void _CheckWasInitialized()
        {
            if (_unitOfWorkFactory == null)
            {
                throw new Exception(
                    "UnitOfWorkHttpModule was not initialized. Call UnitOfWorkHttpModule.Initialize(..) first.");
            }
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            if (HttpContext.Current.Server.GetLastError() != null) return;

            try
            {
                UnitOfWork.Commit();

                DomainEvents.RaiseDelayedEvents();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
            finally
            {
                _unitOfWorkFactory.Release(UnitOfWork);
                UnitOfWork = null;
            }
        }

        private void Application_Error(Object source, EventArgs e)
        {
            if (UnitOfWork == null) return;

            try
            {
                UnitOfWork.Rollback();
            }
            finally
            {
                _unitOfWorkFactory.Release(UnitOfWork);
                UnitOfWork = null;
            }
        }

        private IUnitOfWork UnitOfWork
        {
            get => (IUnitOfWork)HttpContext.Current.Items[UnitOfWorkSessionKey];
            set => HttpContext.Current.Items[UnitOfWorkSessionKey] = value;
        }

        public void Dispose()
        {
        }
    }
}