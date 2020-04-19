﻿using Microsoft.AspNet.Identity;
using SeizeTheDay.Business.Abstract.MySQL;
using SeizeTheDay.Core.Aspects.Postsharp.CacheAspects;
using SeizeTheDay.Core.Aspects.Postsharp.PerformanceAspects;
using SeizeTheDay.Core.CrossCuttingConcerns.Caching.Microsoft;
using SeizeTheDay.DataDomain.Api;
using SeizeTheDay.DataDomain.DTOs;
using SeizeTheDay.DataDomain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ModelPostComment = Xgteamc1XgTeamModel.ForumPostComment;

namespace SeizeTheDay.Api.Controllers
{
    [RoutePrefix("api/postcomments")]
    public class ForumPostCommentsController : ApiController
    {
        #region Ctor
        private readonly IForumPostCommentService _commentService;

        public ForumPostCommentsController(IForumPostCommentService commentService)
        {
            _commentService = commentService;
        }
        #endregion

        [HttpGet]
        [Route("getcomments")]
        [PerformanceCounterAspect]
        [CacheAspect(typeof(MemoryCacheManager), 30)]
        public List<PostCommentDto> GetForumComments(int id)
        {
            List<PostCommentDto> comments = _commentService.StringInclude(id).Select(x => new PostCommentDto()
            {
                CommentID = x.ForumPostCommentID,
                Text = x.Text,
                CreatedTime = System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(x.CreatedTime.Value.Month) + " " +
                x.CreatedTime.Value.Day.ToString() + "," + x.CreatedTime.Value.Year.ToString(),
                CreatedBy = x.CreatedBy,
                ForumPostID = x.ForumPostID,
                CreatedByUserName = x.User.UserName,
                CreatedByUserID = x.User.Id,
                CreatedByPhotoPath = x.User.UserInfoe_Id.PhotoPath,
                CommentLikesCount = x.ForumCommentLikes.Count().ToString()
            }).ToList();

            return comments;
        }

        [Route("createcomment")]
        [HttpPost]
        public IHttpActionResult CreateForumComment([FromBody] ForumPostCommentApi model)
        {
            try
            {
                ModelPostComment newComment = new ModelPostComment
                {
                    Text = model.Text,
                    CreatedTime = DateTime.Now,
                    CreatedBy = User.Identity.GetUserId(),
                    ForumPostID = model.PostID
                };
                _commentService.Add(newComment);
                return Ok(ApiStatusEnum.Ok);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}