using Moq;
using shop_desktop.Models;
using shop_desktop.Services;
using shop_desktop.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tests
{
    public class MainViewModelTests
    {
        [Test]
        public void ExecuteSearch_Should_Filter_Posts_Correctly()
        {
            var posts = new ObservableCollection<Post>
            {
                new Post { Title = "Test Title 1", Content = "Test Content 1" },
                new Post { Title = "Test Title 2", Content = "Test Content 2" },
                new Post { Title = "Sample Title 3", Content = "Sample Content 3" }
            };

            var postService = new PostService();
            postService.AddPost("Test Title 1", "Test Content 1", "Author 1", DateTime.Now);
            postService.AddPost("Test Title 2", "Test Content 2", "Author 2", DateTime.Now);
            postService.AddPost("Sample Title 3", "Sample Content 3", "Author 3", DateTime.Now);

            var viewModel = new MainViewModel(postService);

            viewModel.SearchCommand.Execute("Test");

            Assert.AreEqual(2, viewModel.FilteredPosts.Count);
            Assert.IsTrue(viewModel.FilteredPosts.All(p => p.Title.Contains("Test") || p.Content.Contains("Test")));
        }

        public void SortPosts_Should_Sort_Posts_Correctly()
        {
            var posts = new ObservableCollection<Post>
            {
                new Post { Title = "C", Content = "Test Content 1" },
                new Post { Title = "A", Content = "Test Content 2" },
                new Post { Title = "B", Content = "Sample Content 3" }
            };

            var postService = new PostService();
            postService.AddPost("C", "Test Content 1", "Author 1", DateTime.Now);
            postService.AddPost("A", "Test Content 2", "Author 2", DateTime.Now);
            postService.AddPost("B", "Sample Content 3", "Author 3", DateTime.Now);

            var viewModel = new MainViewModel(postService);

            viewModel.SortPostsCommand.Execute("Tytuł");

            Assert.AreEqual("A", viewModel.Posts[0].Title);
            Assert.AreEqual("B", viewModel.Posts[1].Title);
            Assert.AreEqual("C", viewModel.Posts[2].Title);
        }
    }
}
