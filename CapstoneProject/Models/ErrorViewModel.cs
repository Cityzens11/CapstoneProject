namespace CapstoneProject.Models
{
    /// <summary>
    /// Default ErrorViewModel.
    /// Is not used in project at all.
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}