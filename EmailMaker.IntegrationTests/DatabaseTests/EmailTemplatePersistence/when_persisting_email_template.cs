﻿using System.Linq;
using Core.Tests.Helpers.Persistence;
using EmailMaker.Domain.EmailTemplates;
using EmailMaker.Domain.Users;
using EmailMaker.TestHelper.Builders;
using NUnit.Framework;
using Shouldly;

namespace EmailMaker.IntegrationTests.DatabaseTests.EmailTemplatePersistence
{
    [TestFixture]
    public class when_persisting_email_template : base_simple_persistence_test
    {
        private EmailTemplate _emailTemplate;
        private EmailTemplate _retrievedEmailTemplate;
        private const string TemplateName = "template name";
        private User _user;

        protected override void PersistenceContext()
        {
            _user = UserBuilder.New.Build();
            Save(_user);
            _emailTemplate = new EmailTemplate("html", TemplateName, _user.Id);
            Save(_emailTemplate);
        }

        protected override void PersistenceQuery()
        {
            _retrievedEmailTemplate = Get<EmailTemplate>(_emailTemplate.Id);
        }

        [Test]
        public void email_template_correctly_retrieved()
        {
            _retrievedEmailTemplate.Id.ShouldBe(_emailTemplate.Id);
            _retrievedEmailTemplate.Parts.Count().ShouldBe(_emailTemplate.Parts.Count());
            foreach (var retrievedPart in _retrievedEmailTemplate.Parts)
            {
                var htmlRetrievedPart = retrievedPart as HtmlEmailTemplatePart;
                var htmlPart = _emailTemplate.Parts.First(x => x.Id == htmlRetrievedPart.Id) as HtmlEmailTemplatePart;
                htmlRetrievedPart.Position.ShouldBe(htmlPart.Position);
                htmlRetrievedPart.Html.ShouldBe(htmlPart.Html);
            }
            _retrievedEmailTemplate.Name.ShouldBe(TemplateName);
            _retrievedEmailTemplate.UserId.ShouldBe(_user.Id);
        }
    }
}