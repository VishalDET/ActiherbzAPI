using System;

namespace SynfoShopAPI.Models
{
    public class AddupdateBanner
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string BannerUrl { get; set; }
        public int DisplayOrder { get; set; }
        public int IsCategory { get; set; }
        public bool IsActive { get; set; }
        public string HeadingText { get; set; }
        public string SpType { get; set; }
    }

    public class BannerView
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string BannerUrl { get; set; }
        public int DisplayOrder { get; set; }
        public int IsCategory { get; set; }
        public bool IsActive { get; set; }
        public string HeadingText { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class AddupdateStory
    {
        public int Id { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        public int FileType { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string SpType { get; set; }
    }

    public class StoryView
    {
        public int Id { get; set; }
        public string HeaderText { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int IsCategory { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        public int FileType { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Colors
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public string ColorCode { get; set; }
    }
    public class AddUpdateColor : Colors
    {
        public string SpType { get; set; }
    }
    public class Sizes
    {
        public int Id { get; set; }
        public string Size { get; set; }
    }
    public class AddUpdateSize : Sizes
    {
        public string SpType { get; set; }
    }
}
