using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface ISeedMerchandiseCategoriesTask : ITask<Nothing, bool> { }

    public class SeedMerchandiseCategories : TaskBase, ISeedMerchandiseCategoriesTask
    {
        public SeedMerchandiseCategories(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var merchandiseCategories = _dbContext.MerchandiseCategories.ToList();
                if (merchandiseCategories.Any())
                    return new TaskResult<bool>(false);

                var clothingCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING"),
                    Description = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_HOODIES") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_SWEATERS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_VESTS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_JACKETS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_PANTS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHORTS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_HATS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_SOCKS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHOES") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_UNDERWEAR")
                };
                _dbContext.MerchandiseCategories.Add(clothingCategory);
                _dbContext.SaveChanges();

                var shirtsCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS"),
                    ParentCategoryId = clothingCategory.Id,
                    Description = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_TSHIRTS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_LONG_SLEEVED") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_SLEEVELESS") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_SWEATSHIRTS")
                };
                _dbContext.MerchandiseCategories.Add(shirtsCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_TSHIRTS"),
                    ParentCategoryId = shirtsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_LONG_SLEEVED"),
                    ParentCategoryId = shirtsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_SLEEVELESS"),
                    ParentCategoryId = shirtsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHIRTS_SWEATSHIRTS"),
                    ParentCategoryId = shirtsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_HOODIES"),
                    ParentCategoryId = clothingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SWEATERS"),
                    ParentCategoryId = clothingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_VESTS"),
                    ParentCategoryId = clothingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_JACKETS"),
                    ParentCategoryId = clothingCategory.Id
                });

                var pantsCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_PANTS"),
                    ParentCategoryId = clothingCategory.Id,
                    Description = SeedData("MERCHANDISE_CATEGORY_CLOTHING_PANTS_JEANS") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_PANTS_SWEATPANTS")
                };
                _dbContext.MerchandiseCategories.Add(pantsCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_PANTS_JEANS"),
                    ParentCategoryId = pantsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_PANTS_SWEATPANTS"),
                    ParentCategoryId = pantsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHORTS"),
                    ParentCategoryId = clothingCategory.Id
                });

                var hatsCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_HATS"),
                    ParentCategoryId = clothingCategory.Id,
                    Description = SeedData("MERCHANDISE_CATEGORY_CLOTHING_HATS_BASEBALL") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_CLOTHING_HATS_BEENIES")
                };
                _dbContext.MerchandiseCategories.Add(hatsCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_HATS_BASEBALL"),
                    ParentCategoryId = hatsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_HATS_BEENIES"),
                    ParentCategoryId = hatsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SOCKS"),
                    ParentCategoryId = clothingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_SHOES"),
                    ParentCategoryId = clothingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CLOTHING_UNDERWEAR"),
                    ParentCategoryId = clothingCategory.Id
                });

                var accessoriesCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES"),
                    Description = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_PINS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_PATCHES") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY")
                };
                _dbContext.MerchandiseCategories.Add(accessoriesCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_PINS"),
                    ParentCategoryId = accessoriesCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_PATCHES"),
                    ParentCategoryId = accessoriesCategory.Id
                });

                var jewelryCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY"),
                    ParentCategoryId = accessoriesCategory.Id,
                    Description = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY_BRACELETS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY_NECKLACES") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY_RINGS")
                };
                _dbContext.MerchandiseCategories.Add(jewelryCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY_BRACELETS"),
                    ParentCategoryId = jewelryCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY_NECKLACES"),
                    ParentCategoryId = jewelryCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_ACCESSORIES_JEWELRY_RINGS"),
                    ParentCategoryId = jewelryCategory.Id
                });

                var bagCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_BAGS"),
                    Description = SeedData("MERCHANDISE_CATEGORY_BAGS_TOTES") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_BAGS_PURSES")
                };
                _dbContext.MerchandiseCategories.Add(bagCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_BAGS_TOTES"),
                    ParentCategoryId = bagCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_BAGS_PURSES"),
                    ParentCategoryId = bagCategory.Id
                });

                var containerCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CONTAINERS"),
                    Description = SeedData("MERCHANDISE_CATEGORY_CONTAINERS_JARS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CONTAINERS_BOXES") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CONTAINERS_GLASSES") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CONTAINERS_SHOT_GLASSES") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_CONTAINERS_COOZIES") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_CONTAINERS_MUGS")

                };
                _dbContext.MerchandiseCategories.Add(containerCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CONTAINERS_JARS"),
                    ParentCategoryId = containerCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CONTAINERS_BOXES"),
                    ParentCategoryId = containerCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CONTAINERS_GLASSES"),
                    ParentCategoryId = containerCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CONTAINERS_SHOT_GLASSES"),
                    ParentCategoryId = containerCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CONTAINERS_COOZIES"),
                    ParentCategoryId = containerCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_CONTAINERS_MUGS"),
                    ParentCategoryId = containerCategory.Id
                });

                var printsCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PRINTS"),
                    Description = SeedData("MERCHANDISE_CATEGORY_PRINTS_POSTERS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_PRINTS_COASTERS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_PRINTS_CARDS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_PRINTS_STICKERS") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_PRINTS_FLAGS")
                };
                _dbContext.MerchandiseCategories.Add(printsCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PRINTS_POSTERS"),
                    ParentCategoryId = printsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PRINTS_COASTERS"),
                    ParentCategoryId = printsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PRINTS_CARDS"),
                    ParentCategoryId = printsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PRINTS_STICKERS"),
                    ParentCategoryId = printsCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PRINTS_FLAGS"),
                    ParentCategoryId = printsCategory.Id
                });

                var publishingCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PUBLISHING"),
                    Description = SeedData("MERCHANDISE_CATEGORY_PUBLISHING_HARDBACKS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_PUBLISHING_PAPERBACKS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_PUBLISHING_COMICS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_PUBLISHING_MAGAZINES") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_PUBLISHING_SHEET_MUSIC")
                };
                _dbContext.MerchandiseCategories.Add(publishingCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PUBLISHING_HARDBACKS"),
                    ParentCategoryId = publishingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PUBLISHING_PAPERBACKS"),
                    ParentCategoryId = publishingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PUBLISHING_COMICS"),
                    ParentCategoryId = publishingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PUBLISHING_MAGAZINES"),
                    ParentCategoryId = publishingCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_PUBLISHING_SHEET_MUSIC"),
                    ParentCategoryId = publishingCategory.Id
                });

                var mediaCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_MEDIA"),
                    Description = SeedData("MERCHANDISE_CATEGORY_MEDIA_VINYL") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_MEDIA_CDS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_MEDIA_CASSETTES") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_MEDIA_DVDS") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_MEDIA_BLU_RAYS")
                };
                _dbContext.MerchandiseCategories.Add(mediaCategory);
                _dbContext.SaveChanges();

                var vinylCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_MEDIA_VINYL"),
                    ParentCategoryId = mediaCategory.Id,
                    Description = SeedData("MERCHANDISE_CATEGORY_MEDIA_VINYL_SEVEN_INCH") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_MEDIA_VINYL_TEN_INCH") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_MEDIA_VINYL_TWELVE_INCH")
                };
                _dbContext.MerchandiseCategories.Add(vinylCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_MEDIA_CDS"),
                    ParentCategoryId = mediaCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_MEDIA_CASSETTES"),
                    ParentCategoryId = mediaCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_MEDIA_DVDS"),
                    ParentCategoryId = mediaCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_MEDIA_BLU_RAYS"),
                    ParentCategoryId = mediaCategory.Id
                });

                var toysCategory = new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_TOYS"),
                    Description = SeedData("MERCHANDISE_CATEGORY_TOYS_DOLLS") + ", " +
                                  SeedData("MERCHANDISE_CATEGORY_TOYS_FIGURINES") + " " +
                                  CommonWord("AND") + " " +
                                  SeedData("MERCHANDISE_CATEGORY_TOYS_PUZZLES")
                };
                _dbContext.MerchandiseCategories.Add(toysCategory);
                _dbContext.SaveChanges();

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_TOYS_DOLLS"),
                    ParentCategoryId = toysCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_TOYS_FIGURINES"),
                    ParentCategoryId = toysCategory.Id
                });

                _dbContext.MerchandiseCategories.Add(new MerchandiseCategory
                {
                    Name = SeedData("MERCHANDISE_CATEGORY_TOYS_PUZZLES"),
                    ParentCategoryId = toysCategory.Id
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
