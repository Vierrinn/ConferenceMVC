using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private record CountByTopicResponseItem(string Topic, int Count);

        private readonly DbconferenceContext conferenceContext;

        public ChartController(DbconferenceContext conferenceContext)
        {
            this.conferenceContext = conferenceContext;
        }
        
        [HttpGet("countByTopic")]
        public async Task<JsonResult> GetCountByTopicAsync(CancellationToken cancellationToken)
        {
            var responseItems = await conferenceContext
                .Conferences
                .GroupBy(conference => conference.Topic.Name)
                .Select(group => new CountByTopicResponseItem(group.Key.ToString(), group.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
        
        [HttpGet("countUsersByTopic")]
        public async Task<JsonResult> GetCountUsersByTopicAsync(CancellationToken cancellationToken)
        {
            var responseItems = await conferenceContext.Conferences
                .Include(c => c.Topic)
                .Include(c => c.SignUps)
                .GroupBy(conference => conference.Topic.Name)
                .Select(group => new
                {
                    Topic = group.Key,
                    UsersCount = group.SelectMany(conference => conference.SignUps).Count()
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }



    }
}
