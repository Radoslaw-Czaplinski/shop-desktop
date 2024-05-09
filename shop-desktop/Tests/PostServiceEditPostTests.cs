using NUnit.Framework;
using shop_desktop.Models;
using shop_desktop.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tests
{
    public class PostServiceEditPostTests
    {
        [Test]
        public void UpdatePost_Updates_Existing_Post()
        {
            var postService = new PostService();
            var initialTitle = "Initial Title";
            var updatedTitle = "Updated Title";
            var content = "Sample Content";
            var author = "Author";
            var dateAdded = DateTime.Now;

            postService.AddPost(initialTitle, content, author, dateAdded);

            var posts = postService.LoadPosts();
            var initialPost = posts.FirstOrDefault(p => p.Title == initialTitle);
            Assert.NotNull(initialPost, "Initial post not found");

            initialPost.Title = updatedTitle;
            postService.UpdatePost(initialPost);

            var updatedPosts = postService.LoadPosts();
            var updatedPost = updatedPosts.FirstOrDefault(p => p.Title == updatedTitle);

            Assert.NotNull(updatedPost, "Updated post not found");
            Assert.AreEqual(updatedTitle, updatedPost.Title, "Post title not updated correctly");
            Assert.AreEqual(content, updatedPost.Content, "Post content not updated correctly");
            Assert.AreEqual(author, updatedPost.Author, "Post author not updated correctly");
            Assert.AreEqual(dateAdded, updatedPost.DateAdded, "Post date added not updated correctly");
        }
    }
}
