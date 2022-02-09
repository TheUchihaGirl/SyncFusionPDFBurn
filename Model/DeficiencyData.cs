using Microsoft.AspNetCore.Http;

namespace SyncFusionPDFBurning.Model
{
    public class DeficiencyData : Metadata
    {
        public IFormFile file { get; set; }
    }
}