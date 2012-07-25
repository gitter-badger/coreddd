﻿using EmailMaker.Commands.Messages;
using EmailMaker.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace EmailMaker.UnitTests.Controllers.Templates
{
    [TestFixture]
    public class when_creating_a_variable : base_emailmaker_controller_test
    {
        private CreateVariableCommand _createVariableCommand;

        public override void Context()
        {
            var controller = new TemplateController(CommandExecutor, null);
            _createVariableCommand = new CreateVariableCommand();
            controller.CreateVariable(_createVariableCommand);
        }

        [Test]
        public void command_was_executed()
        {
            CommandExecutor.AssertWasCalled(a => a.Execute(_createVariableCommand));
        }
    }
}