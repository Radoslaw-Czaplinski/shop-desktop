using NUnit.Framework;
using shop_desktop.Models;
using shop_desktop.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tests
{
    public class PostServiceDeletePostTests
    {
        [Test]
        public void DeletePost_Removes_Existing_Post()
        {
            var postService = new PostService();
            var title = "Sample Title";
            var content = "Sample Content";
            var author = "Author";
            var dateAdded = DateTime.Now;

            postService.AddPost(title, content, author, dateAdded);

            var posts = postService.LoadPosts();
            var addedPost = posts.FirstOrDefault(p => p.Title == title);
            Assert.NotNull(addedPost, "Added post not found");

            postService.DeletePost(addedPost);

            var updatedPosts = postService.LoadPosts();
            var deletedPost = updatedPosts.FirstOrDefault(p => p.Title == title);
            Assert.IsNull(deletedPost, "Deleted post found in the list of posts");
        }

        [Test]
        public void DeletePost_Handles_Errors_Correctly()
        {
            var postService = new PostService();
            var nonExistingPost = new Post { Title = "Non Existing Title", Content = "Non Existing Content", Author = "Non Existing Author", DateAdded = DateTime.Now };

            postService.DeletePost(nonExistingPost);

            var loadedPosts = postService.LoadPosts();
            Assert.AreEqual(2, loadedPosts.Count); 
        }


    }
}
