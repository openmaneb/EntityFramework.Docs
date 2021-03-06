﻿using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EFQuerying.RelatedData
{
    public class Sample
    {
        public static void Run()
        {
            #region SingleInclude
            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .ToList();
            }
            #endregion

            #region IgnoredInclude
            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .Select(blog => new
                    {
                        Id = blog.BlogId,
                        Url = blog.Url
                    })
                    .ToList();
            }
            #endregion

            #region MultipleIncludes
            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .Include(blog => blog.Owner)
                    .ToList();
            }
            #endregion

            #region SingleThenInclude
            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                        .ThenInclude(post => post.Author)
                    .ToList();
            }
            #endregion

            #region MultipleThenIncludes
            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                        .ThenInclude(post => post.Author)
                            .ThenInclude(author => author.Photo)
                    .ToList();
            }
            #endregion

            #region MultipleLeafIncludes
            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                        .ThenInclude(post => post.Author)
                    .Include(blog => blog.Posts)
                        .ThenInclude(post => post.Tags)
                    .ToList();
            }
            #endregion

            #region IncludeTree
            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                        .ThenInclude(post => post.Author)
                        .ThenInclude(author => author.Photo)
                    .Include(blog => blog.Owner)
                        .ThenInclude(owner => owner.Photo)
                    .ToList();
            }
            #endregion
            
            #region Eager
            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                    .Single(b => b.BlogId == 1);

                context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Load();

                context.Entry(blog)
                    .Reference(b => b.Owner)
                    .Load();
            }
            #endregion

            #region NavQueryAggregate
            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                    .Single(b => b.BlogId == 1);

                var postCount = context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Query()
                    .Count();
            }
            #endregion

            #region NavQueryFiltered
            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                    .Single(b => b.BlogId == 1);

                var goodPosts = context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Query()
                    .Where(p => p.Rating > 3)
                    .ToList();
            }
            #endregion
        }
    }
}
