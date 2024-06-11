namespace HelpDesk.AppService.Web.Models.TicketViewModels
{
    public sealed class TicketActionViewModel
    {
        public int IdTicket { get; set; }
        public byte IdActionType { get; set; }
        public int? IdUserAssigned { get; set; }
        public byte? IdStatusChanged { get; set; }
        public string CancellationReason { get; set; }

        public int IdCategory { get; set; }
        public string Description { get; set; }
    }
}
