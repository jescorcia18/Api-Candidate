using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestPandape.API.Controllers;
using TestPandape.Business.IServices;
using TestPandape.Entity.Candidates;
using TestPandape.Entity.Message;

namespace TestPandape.UnitTest.Controllers
{
    [TestFixture]
    public class CandidatesControllerTest
    {
        CandidatesController testCandidateController;
        private Mock<ICandidateBL> mockCandidateController;

        [SetUp]
        public void SetUp()
        {
            mockCandidateController = new Mock<ICandidateBL>();
            testCandidateController = new CandidatesController(mockCandidateController.Object);
        }

        [Test]
        public async Task CreateCandidate_Success()
        {
            //Arrange
            int? mockId = 1;
            bool mockSuccess = true;
            string mockMessage = "Candidate successfully registered.";
            CandidateRequest request = new CandidateRequest
            {
                Name= "Juan",
                Surname="Escorcia",
                Birthdate= new DateTime( 1990,01,01),
                Email="juanesru@hotmail.com"
            };

            CandidateResponse response = new CandidateResponse
            {
                IdCandidate = mockId,
                MessageResponse = new MessageResponse { message = mockMessage, success = mockSuccess }
            };
            mockCandidateController.Setup(c => c.CreateCandidateService(request)).ReturnsAsync(response);

            //Act
            var result = await testCandidateController.Create(request) as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            var objResult = result.Value as CandidateResponse;
            Assert.IsNotNull(objResult);
            Assert.That(objResult.IdCandidate.Equals(mockId));
            Assert.That(objResult.MessageResponse.success.Equals(mockSuccess));
            Assert.That(objResult.MessageResponse.message.Equals(mockMessage));

            mockCandidateController.Verify(m=>m.CreateCandidateService(request),Times.Once());
        }

        [Test]
        public async Task CreateCandidate_Failured()
        {
            //Arrange
            int? mockId = null;
            bool mockSuccess = false;
            string mockMessage = "Could not create Candidate.";
            CandidateRequest request = new CandidateRequest
            {
                Name = "Juan",
                Surname = "Escorcia",
                Birthdate = new DateTime(1990, 01, 01),
                Email = "juanesru@hotmail.com"
            };

            CandidateResponse response = new CandidateResponse
            {
                IdCandidate = mockId,
                MessageResponse = new MessageResponse { message = mockMessage, success = mockSuccess }
            };
            mockCandidateController.Setup(c => c.CreateCandidateService(request)).ThrowsAsync(new Exception(mockMessage));

            //Act
            var result = await testCandidateController.Create(request) as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result?.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value.Equals(mockMessage));

            mockCandidateController.Verify(m => m.CreateCandidateService(request), Times.Once());
        }

        [Test]
        public async Task CreateCandidate_ValidateFieldsEmailIncorrect()
        {
            //Arrange
            int? mockId = null;
            bool mockSuccess = false;
            string mockMessage = "One or more fields do not have the allowed value";
            CandidateRequest request = new CandidateRequest
            {
                Name = "Juan",
                Surname = "Escorcia",
                Birthdate = new DateTime(1990, 01, 01),
                Email = "juanesru@hotmail"
            };

            CandidateResponse response = new CandidateResponse
            {
                IdCandidate = mockId,
                MessageResponse = new MessageResponse { message = mockMessage, success = mockSuccess }
            };
            mockCandidateController.Setup(c => c.CreateCandidateService(request)).ReturnsAsync(response);

            //Act
            var result = await testCandidateController.Create(request) as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            var objResult = result.Value as CandidateResponse;
            Assert.IsNotNull(objResult);
            Assert.That(objResult.IdCandidate.Equals(mockId));
            Assert.That(objResult.MessageResponse.success.Equals(mockSuccess));
            Assert.That(objResult.MessageResponse.message.Equals(mockMessage));

            mockCandidateController.Verify(m => m.CreateCandidateService(request), Times.Once());
        }

        [Test]
        public async Task CreateCandidate_IfExistCandidate()
        {
            //Arrange
            int? mockId = 1;
            bool mockSuccess = false;
            string mockMessage = "Candidate is already registered with this email.";
            CandidateRequest request = new CandidateRequest
            {
                Name = "Juan",
                Surname = "Escorcia",
                Birthdate = new DateTime(1990, 01, 01),
                Email = "juanesru@hotmail"
            };

            CandidateResponse response = new CandidateResponse
            {
                IdCandidate = mockId,
                MessageResponse = new MessageResponse { message = mockMessage, success = mockSuccess }
            };
            mockCandidateController.Setup(c => c.CreateCandidateService(request)).ReturnsAsync(response);

            //Act
            var result = await testCandidateController.Create(request) as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            var objResult = result.Value as CandidateResponse;
            Assert.IsNotNull(objResult);
            Assert.That(objResult.IdCandidate.Equals(mockId));
            Assert.That(objResult.MessageResponse.success.Equals(mockSuccess));
            Assert.That(objResult.MessageResponse.message.Equals(mockMessage));

            mockCandidateController.Verify(m => m.CreateCandidateService(request), Times.Once());
        }
    }
}
