using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Application.Schools.Commands.DeleteSchool;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Schools.Commands.CreateSchool;

public class DeleteSchoolCommandHandler(ISchoolRepository schoolRepository, IAlbumRepository albumRepository) : IRequestHandler<DeleteSchoolCommand, Result>
{
    private readonly ISchoolRepository _schoolRepository = schoolRepository;
    private readonly IAlbumRepository _albumRepository = albumRepository;
    public async Task<Result> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var albums = await _albumRepository.FindBySchoolIdAsync(request.Id);
            if (albums != null && albums.Any())
            {
                foreach (var album in albums)
                {
                    await _albumRepository.DeleteAsync(album.Id);
                }
            }

            await _schoolRepository.DeleteAsync(request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failure to delete school: {ex.Message}");
        }
    }
}
