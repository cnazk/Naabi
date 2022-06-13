using System.ComponentModel.DataAnnotations;
using Common;

namespace FriendsService.Models.DataBase;

public class FriendRequest : NonDeletableEntity
{
    [Required] public string From { get; set; }
    [Required] public string To { get; set; }
    public bool Responded { get; set; } = false;
    public bool IsAccepted { get; set; } = false;
    public DateTime? RespondDateTime { get; set; }
}