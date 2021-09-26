using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface ISeedGenresTask : ITask<Nothing, bool> { }

    public class SeedGenres : TaskBase, ISeedGenresTask
    {
        public SeedGenres(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var genres = _dbContext.Genres.ToList();
                if (genres.Any())
                    return new TaskResult<bool>(false);

                var rockGenre = new Genre
                {
                    Name = SeedData("GENRE_ROCK")
                };
                _dbContext.Genres.Add(rockGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_CLASSIC_ROCK"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_HARD_ROCK"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_FOLK_ROCK"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_GOTH_ROCK"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_GRUNGE"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.SaveChanges();

                var indieRockGenre = new Genre
                {
                    Name = SeedData("GENRE_ROCK_INDIE_ROCK")
                };
                _dbContext.Genres.Add(indieRockGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_INDIE_ROCK_LO_FI"),
                    ParentGenreId = indieRockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_INDIE_ROCK_NEW_WAVE"),
                    ParentGenreId = indieRockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_INDIE_ROCK_RETRO_VAGUE"),
                    ParentGenreId = indieRockGenre.Id
                });

                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_PROGRESSIVE_ROCK"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_PSYCHEDELIC_ROCK"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_POP_ROCK"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_ROCK_ROCKABILLY"),
                    ParentGenreId = rockGenre.Id
                });

                _dbContext.SaveChanges();

                var popGenre = new Genre
                {
                    Name = SeedData("GENRE_POP")
                };
                _dbContext.Genres.Add(popGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_POP_ADULT_CONTEMPORARY"),
                    ParentGenreId = popGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_POP_BRITPOP"),
                    ParentGenreId = popGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_POP_ELECTROPOP"),
                    ParentGenreId = popGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_POP_INDIE_POP"),
                    ParentGenreId = popGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_POP_OLDIES"),
                    ParentGenreId = popGenre.Id
                });

                _dbContext.SaveChanges();

                var punkGenre = new Genre
                {
                    Name = SeedData("GENRE_PUNK")
                };
                _dbContext.Genres.Add(punkGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_PUNK_POP_PUNK"),
                    ParentGenreId = punkGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_PUNK_HARDCORE"),
                    ParentGenreId = punkGenre.Id
                });

                _dbContext.SaveChanges();

                var hipHopGenre = new Genre
                {
                    Name = SeedData("GENRE_HIP_HOP")
                };
                _dbContext.Genres.Add(hipHopGenre);
                _dbContext.SaveChanges();

                var rapGenre = new Genre
                {
                    Name = SeedData("GENRE_HIP_HOP_RAP"),
                    ParentGenreId = hipHopGenre.Id
                };
                _dbContext.Genres.Add(rapGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_HIP_HOP_RAP_HARDCORE"),
                    ParentGenreId = rapGenre.Id
                });

                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_HIP_HOP_TRAP"),
                    ParentGenreId = hipHopGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_HIP_HOP_UNDERGROUND"),
                    ParentGenreId = hipHopGenre.Id
                });

                _dbContext.SaveChanges();

                var danceElectronicGenre = new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC")
                };
                _dbContext.Genres.Add(danceElectronicGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_BIG_BEAT"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_DOWNTEMPO"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_HOUSE"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_JUNGLE"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_TECHNO"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_TRANCE"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_TRIP_HOP"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_DANCE_ELECTRONIC_DRUM_AND_BASS"),
                    ParentGenreId = danceElectronicGenre.Id
                });

                _dbContext.SaveChanges();

                var countryGenre = new Genre
                {
                    Name = SeedData("GENRE_COUNTRY")
                };
                _dbContext.Genres.Add(countryGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_COUNTRY_BLUEGRASS"),
                    ParentGenreId = countryGenre.Id
                });

                _dbContext.SaveChanges();

                var folkGenre = new Genre
                {
                    Name = SeedData("GENRE_FOLK")
                };
                _dbContext.Genres.Add(folkGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_FOLK_SKIFFLE"),
                    ParentGenreId = folkGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_FOLK_POLKA"),
                    ParentGenreId = folkGenre.Id
                });

                _dbContext.SaveChanges();

                var jazzGenre = new Genre
                {
                    Name = SeedData("GENRE_JAZZ")
                };
                _dbContext.Genres.Add(jazzGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_JAZZ_AVANT_GARDE"),
                    ParentGenreId = jazzGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_JAZZ_BEBOP"),
                    ParentGenreId = jazzGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_JAZZ_BIG_BAND"),
                    ParentGenreId = jazzGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_JAZZ_DIXIELAND"),
                    ParentGenreId = jazzGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_JAZZ_SMOOTH"),
                    ParentGenreId = jazzGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_JAZZ_VOCAL"),
                    ParentGenreId = jazzGenre.Id
                });

                _dbContext.SaveChanges();

                var bluesGenre = new Genre
                {
                    Name = SeedData("GENRE_BLUES")
                };
                _dbContext.Genres.Add(bluesGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_BLUES_DELTA_BLUES"),
                    ParentGenreId = bluesGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_BLUES_MEMPHIS_BLUES"),
                    ParentGenreId = bluesGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_BLUES_CHICAGO_BLUES"),
                    ParentGenreId = bluesGenre.Id
                });

                _dbContext.SaveChanges();

                var rNbGenre = new Genre
                {
                    Name = SeedData("GENRE_R&B")
                };
                _dbContext.Genres.Add(rNbGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_R&B_SOUL"),
                    ParentGenreId = rNbGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_R&B_FUNK"),
                    ParentGenreId = rNbGenre.Id
                });

                _dbContext.SaveChanges();

                var metalGenre = new Genre
                {
                    Name = SeedData("GENRE_METAL")
                };
                _dbContext.Genres.Add(metalGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_METAL_BLACK_METAL"),
                    ParentGenreId = metalGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_METAL_DEATH_METAL"),
                    ParentGenreId = metalGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_METAL_NU_METAL"),
                    ParentGenreId = metalGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_METAL_THRASH_METAL"),
                    ParentGenreId = metalGenre.Id
                });

                _dbContext.SaveChanges();

                var industrialGenre = new Genre
                {
                    Name = SeedData("GENRE_INDUSTRIAL")
                };
                _dbContext.Genres.Add(industrialGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_INDUSTRIAL_KRAUTROCK"),
                    ParentGenreId = industrialGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_INDUSTRIAL_DARK_WAVE"),
                    ParentGenreId = industrialGenre.Id
                });

                _dbContext.SaveChanges();

                var newAgeGenre = new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE")
                };
                _dbContext.Genres.Add(newAgeGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE_AMBIENT"),
                    ParentGenreId = newAgeGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE_POST_ROCK"),
                    ParentGenreId = newAgeGenre.Id
                });

                _dbContext.SaveChanges();

                var worldGenre = new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE_WORLD"),
                    ParentGenreId = newAgeGenre.Id
                };
                _dbContext.Genres.Add(worldGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE_WORLD_AFROBEAT"),
                    ParentGenreId = worldGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE_WORLD_CELTIC"),
                    ParentGenreId = worldGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE_WORLD_ZYDECO"),
                    ParentGenreId = worldGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_NEW_AGE_WORLD_INDIAN"),
                    ParentGenreId = worldGenre.Id
                });

                _dbContext.SaveChanges();

                var classicalGenre = new Genre
                {
                    Name = SeedData("GENRE_CLASSICAL")
                };
                _dbContext.Genres.Add(classicalGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_CLASSICAL_BAROQUE"),
                    ParentGenreId = classicalGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_CLASSICAL_ROMANTIC"),
                    ParentGenreId = classicalGenre.Id
                });

                _dbContext.SaveChanges();

                var vocalGenre = new Genre
                {
                    Name = SeedData("GENRE_VOCAL")
                };
                _dbContext.Genres.Add(vocalGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_VOCAL_MUSICAL"),
                    ParentGenreId = vocalGenre.Id
                });

                var spokenWordGenre = new Genre
                {
                    Name = SeedData("GENRE_VOCAL_SPOKEN_WORD"),
                    ParentGenreId = vocalGenre.Id
                };
                _dbContext.Genres.Add(spokenWordGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_VOCAL_SPOKEN_WORD_COMEDY"),
                    ParentGenreId = spokenWordGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_VOCAL_SPOKEN_WORD_POETRY"),
                    ParentGenreId = spokenWordGenre.Id
                });

                _dbContext.SaveChanges();

                var reggaeGenre = new Genre
                {
                    Name = SeedData("GENRE_REGGAE")
                };
                _dbContext.Genres.Add(reggaeGenre);
                _dbContext.SaveChanges();

                var dubGenre = new Genre
                {
                    Name = SeedData("GENRE_REGGAE_DUB"),
                    ParentGenreId = reggaeGenre.Id
                };
                _dbContext.Genres.Add(dubGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_REGGAE_DUB_DUBSTEP"),
                    ParentGenreId = dubGenre.Id
                });

                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_REGGAE_ROCKSTEADY"),
                    ParentGenreId = reggaeGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_REGGAE_DANCEHALL"),
                    ParentGenreId = reggaeGenre.Id
                });

                _dbContext.SaveChanges();

                var latinGenre = new Genre
                {
                    Name = SeedData("GENRE_LATIN")
                };
                _dbContext.Genres.Add(latinGenre);
                _dbContext.SaveChanges();

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_LATIN_SALSA"),
                    ParentGenreId = latinGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_LATIN_BOSSA_NOVA"),
                    ParentGenreId = latinGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_LATIN_RUMBA"),
                    ParentGenreId = latinGenre.Id
                });

                _dbContext.Genres.Add(new Genre
                {
                    Name = SeedData("GENRE_LATIN_MERENGUE"),
                    ParentGenreId = latinGenre.Id
                });

                _dbContext.SaveChanges();

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
