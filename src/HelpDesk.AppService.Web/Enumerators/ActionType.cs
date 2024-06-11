namespace HelpDesk.AppService.Web.Enumerators
{
    internal enum ActionType : byte
    {
        Create = 1,
        Edit = 2,
        AssignTo = 3,
        AssignToMe = 4,
        ChangeStatusTo = 5,
        ChangeStatusToComplete = 6,
        Cancellation = 7
    }
}
