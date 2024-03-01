using ArtworkSharing.Core.Domain.Base;

namespace ArtworkSharing.Core.Domain.Entities
{
    public class Role : EntityBase<int>
    {
        public string Name { get; set; } = null!;
       
    }
}
