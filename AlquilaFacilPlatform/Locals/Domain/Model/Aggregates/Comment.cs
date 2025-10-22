using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;

namespace AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;

public partial class Comment
{
    public Comment()
    {
        UserId = 0;
        LocalId = 0;
        Text = new TextComment();
        Rating = new RatingComment();
    }

    public Comment(int userId, int localId, string text, int rating)
    {
        UserId = userId;
        LocalId = localId;
        Text = new TextComment(text);
        Rating = new RatingComment(rating);
    }

    public Comment(CreateCommentCommand command)
    {
        UserId = command.UserId;
        LocalId = command.LocalId;
        Text = new TextComment(command.Text);
        Rating = new RatingComment(command.Rating);
    }
    
    public int Id { get; }
    public int UserId { get; set; }
    public int LocalId { get; set; }
    public TextComment Text { get; set; }
    public RatingComment Rating { get; set; }

    public string CommentText => Text.Value;

    public int CommentRating => Rating.Value;

}