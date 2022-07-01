using System;
using System.Collections.Generic;
using NUnit.Framework;
using TripIt2Gether.Models;

namespace TripIt2Gether.UnitTests.Models
{
    public class Application
    {

        [TestCase(ParticipationStatus.OczekujacaPotiwerdzenia, ParticipationStatus.OczekujacaPotiwerdzenia, false)]
        [TestCase(ParticipationStatus.OczekujacaPotiwerdzenia, ParticipationStatus.Anulowana, true)]
        [TestCase(ParticipationStatus.OczekujacaPotiwerdzenia, ParticipationStatus.Zaakaceptowana, true)]
        [TestCase(ParticipationStatus.OczekujacaPotiwerdzenia, ParticipationStatus.NieZrealizowana, false)]
        [TestCase(ParticipationStatus.OczekujacaPotiwerdzenia, ParticipationStatus.Odbyta, false)]
        [TestCase(ParticipationStatus.Anulowana, ParticipationStatus.OczekujacaPotiwerdzenia, false)]
        [TestCase(ParticipationStatus.Anulowana, ParticipationStatus.Anulowana, false)]
        [TestCase(ParticipationStatus.Anulowana, ParticipationStatus.Zaakaceptowana, true)]
        [TestCase(ParticipationStatus.Anulowana, ParticipationStatus.NieZrealizowana, false)]
        [TestCase(ParticipationStatus.Anulowana, ParticipationStatus.Odbyta, false)]
        [TestCase(ParticipationStatus.Zaakaceptowana, ParticipationStatus.OczekujacaPotiwerdzenia, false)]
        [TestCase(ParticipationStatus.Zaakaceptowana, ParticipationStatus.Anulowana, true)]
        [TestCase(ParticipationStatus.Zaakaceptowana, ParticipationStatus.Zaakaceptowana, false)]
        [TestCase(ParticipationStatus.Zaakaceptowana, ParticipationStatus.NieZrealizowana, true)]
        [TestCase(ParticipationStatus.Zaakaceptowana, ParticipationStatus.Odbyta, true)]
        public void ChangegStatus_CheckifcanStatusTests(ParticipationStatus roleStatus, ParticipationStatus newStatus, bool expectedResults)
        {
            var application = new TripIt2Gether.Models.Application() { Status = roleStatus };
            var res = application.ChangegStatus(newStatus);
            Assert.That(res, Is.EqualTo(expectedResults));
        }

        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(0, 1, true)]
        [TestCase(1, 1, true)]
        [TestCase(2, 5, true)]
        [TestCase(5, 2, false)]
        [TestCase(5, 5, true)]
        public void CheckIfUserAnsweredForAllQuestions(int numberOfQuestions, int numberOfAnswears, bool expectedRes)
        {
            var application = new TripIt2Gether.Models.Application() { Answers = new List<Answer>(), Trip = new Trip() { Form = new Form() { Questions = new List<Question>()} } };
            for (int i = 0; i < numberOfAnswears; i++)
            {
                application.Answers.Add(new Answer());
            }

            for (int i = 0; i < numberOfQuestions; i++)
            {
                application.Trip.Form.Questions.Add(new Question());
            }

            var res = application.CheckIfUserAnsweredForAllQuestions();
            Assert.That(res, Is.EqualTo(expectedRes));
        }
    }
}
