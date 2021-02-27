using AutoMapper;
using DocKick.Dtos.Blobs;
using DocKick.Entities.Blobs;

namespace DocKick.Mapper
{
    public class BlobProfile : Profile
    {
        public BlobProfile()
        {
            CreateMap<Blob, BlobModel>();
        }
    }
}