public class Video
{
    public string Title  { get; set; }
    public string Author { get; set; }
    public int    Length { get; set; }   // in seconds

    private List<Comment> _comments = new List<Comment>();

    public Video(string title, string author, int length)
    {
        Title  = title;
        Author = author;
        Length = length;
    }

    // Add a comment to this video
    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    // Return the number of comments
    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    // Give Program.cs read-only access to iterate the comments
    public IEnumerable<Comment> GetComments()
    {
        return _comments;
    }
}
