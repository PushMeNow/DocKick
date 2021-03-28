using AutoMapper;
using DocKick.Dtos.Blobs;
using DocKick.Entities.Blobs;

namespace DocKick.Mapper
{
    public class BlobProfile : Profile
    {
        public BlobProfile()
        {
            CreateMap<BlobRequest, BlobModel>();
            CreateMap<Blob, BlobModel>();
            CreateMap<BlobModel, Blob>()
                .ForMember(q => q.BlobId, q => q.Ignore())
                .ForMember(q => q.BlobLink, q => q.Ignore());

            CreateMap<BlobLink, BlobLinkModel>();
        }
    }
}