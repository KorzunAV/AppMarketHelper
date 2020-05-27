using System;
using Newtonsoft.Json;

namespace AppMarketHelper.PlayGoogleCom.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppInfo : IComparable<AppInfo>
    {
        [JsonProperty("IAPRange")]
        public string IAPRange { get; set; }

        [JsonProperty("adSupported")]
        public string AdSupported { get; set; }

        [JsonProperty("androidVersion")]
        public string AndroidVersion { get; set; }

        [JsonProperty("androidVersionText")]
        public string AndroidVersionText { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("comments")]
        public string[] Comments { get; set; }

        [JsonProperty("contentRating")]
        public string ContentRating { get; set; }

        [JsonProperty("contentRatingDescription")]
        public string ContentRatingDescription { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("descriptionHTML")]
        public string DescriptionHTML { get; set; }

        [JsonProperty("developer")]
        public string Developer { get; set; }

        [JsonProperty("developerAddress")]
        public string DeveloperAddress { get; set; }

        [JsonProperty("developerEmail")]
        public string DeveloperEmail { get; set; }

        [JsonProperty("developerId")]
        public string DeveloperId { get; set; }

        [JsonProperty("developerInternalID")]
        public string DeveloperInternalID { get; set; }

        [JsonProperty("developerWebsite")]
        public string DeveloperWebsite { get; set; }

        [JsonProperty("editorsChoice")]
        public bool EditorsChoice { get; set; }

        [JsonProperty("familyGenre")]
        public string FamilyGenre { get; set; }

        [JsonProperty("familyGenreId")]
        public string FamilyGenreId { get; set; }

        [JsonProperty("free")]
        public bool Free { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("genreId")]
        public string GenreId { get; set; }

        [JsonProperty("headerImage")]
        public string HeaderImage { get; set; }

        [JsonProperty("histogram")]
        public Histogram Histogram { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("installs")]
        public string Installs { get; set; }

        [JsonProperty("minInstalls")]
        public long MinInstalls { get; set; }

        [JsonProperty("offersIAP")]
        public string OffersIAP { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("priceText")]
        public string PriceText { get; set; }

        [JsonProperty("privacyPolicy")]
        public string PrivacyPolicy { get; set; }

        [JsonProperty("ratings")]
        public long Ratings { get; set; }

        [JsonProperty("recentChanges")]
        public string RecentChanges { get; set; }

        [JsonProperty("released")]
        public string Released { get; set; }

        [JsonProperty("reviews")]
        public long Reviews { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("scoreText")]
        public string ScoreText { get; set; }

        [JsonProperty("screenshots")]
        public string[] Screenshots { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("video")]
        public string Video { get; set; }

        [JsonProperty("videoImage")]
        public string VideoImage { get; set; }

        public int CompareTo(AppInfo other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var iapRangeComparison = string.Compare(IAPRange, other.IAPRange, StringComparison.Ordinal);
            if (iapRangeComparison != 0) return iapRangeComparison;
            var adSupportedComparison = AdSupported.CompareTo(other.AdSupported);
            if (adSupportedComparison != 0) return adSupportedComparison;
            var androidVersionComparison = string.Compare(AndroidVersion, other.AndroidVersion, StringComparison.Ordinal);
            if (androidVersionComparison != 0) return androidVersionComparison;
            var androidVersionTextComparison = string.Compare(AndroidVersionText, other.AndroidVersionText, StringComparison.Ordinal);
            if (androidVersionTextComparison != 0) return androidVersionTextComparison;
            var appIdComparison = string.Compare(AppId, other.AppId, StringComparison.Ordinal);
            if (appIdComparison != 0) return appIdComparison;
            var contentRatingComparison = string.Compare(ContentRating, other.ContentRating, StringComparison.Ordinal);
            if (contentRatingComparison != 0) return contentRatingComparison;
            var contentRatingDescriptionComparison = string.Compare(ContentRatingDescription, other.ContentRatingDescription, StringComparison.Ordinal);
            if (contentRatingDescriptionComparison != 0) return contentRatingDescriptionComparison;
            var currencyComparison = string.Compare(Currency, other.Currency, StringComparison.Ordinal);
            if (currencyComparison != 0) return currencyComparison;
            var descriptionComparison = string.Compare(Description, other.Description, StringComparison.Ordinal);
            if (descriptionComparison != 0) return descriptionComparison;
            var descriptionHtmlComparison = string.Compare(DescriptionHTML, other.DescriptionHTML, StringComparison.Ordinal);
            if (descriptionHtmlComparison != 0) return descriptionHtmlComparison;
            var developerComparison = string.Compare(Developer, other.Developer, StringComparison.Ordinal);
            if (developerComparison != 0) return developerComparison;
            var developerAddressComparison = string.Compare(DeveloperAddress, other.DeveloperAddress, StringComparison.Ordinal);
            if (developerAddressComparison != 0) return developerAddressComparison;
            var developerEmailComparison = string.Compare(DeveloperEmail, other.DeveloperEmail, StringComparison.Ordinal);
            if (developerEmailComparison != 0) return developerEmailComparison;
            var developerIdComparison = string.Compare(DeveloperId, other.DeveloperId, StringComparison.Ordinal);
            if (developerIdComparison != 0) return developerIdComparison;
            var developerInternalIdComparison = string.Compare(DeveloperInternalID, other.DeveloperInternalID, StringComparison.Ordinal);
            if (developerInternalIdComparison != 0) return developerInternalIdComparison;
            var developerWebsiteComparison = string.Compare(DeveloperWebsite, other.DeveloperWebsite, StringComparison.Ordinal);
            if (developerWebsiteComparison != 0) return developerWebsiteComparison;
            var editorsChoiceComparison = EditorsChoice.CompareTo(other.EditorsChoice);
            if (editorsChoiceComparison != 0) return editorsChoiceComparison;
            var familyGenreComparison = string.Compare(FamilyGenre, other.FamilyGenre, StringComparison.Ordinal);
            if (familyGenreComparison != 0) return familyGenreComparison;
            var familyGenreIdComparison = string.Compare(FamilyGenreId, other.FamilyGenreId, StringComparison.Ordinal);
            if (familyGenreIdComparison != 0) return familyGenreIdComparison;
            var freeComparison = Free.CompareTo(other.Free);
            if (freeComparison != 0) return freeComparison;
            var genreComparison = string.Compare(Genre, other.Genre, StringComparison.Ordinal);
            if (genreComparison != 0) return genreComparison;
            var genreIdComparison = string.Compare(GenreId, other.GenreId, StringComparison.Ordinal);
            if (genreIdComparison != 0) return genreIdComparison;
            var headerImageComparison = string.Compare(HeaderImage, other.HeaderImage, StringComparison.Ordinal);
            if (headerImageComparison != 0) return headerImageComparison;
            var iconComparison = string.Compare(Icon, other.Icon, StringComparison.Ordinal);
            if (iconComparison != 0) return iconComparison;
            var installsComparison = string.Compare(Installs, other.Installs, StringComparison.Ordinal);
            if (installsComparison != 0) return installsComparison;
            var minInstallsComparison = MinInstalls.CompareTo(other.MinInstalls);
            if (minInstallsComparison != 0) return minInstallsComparison;
            var offersIapComparison = OffersIAP.CompareTo(other.OffersIAP);
            if (offersIapComparison != 0) return offersIapComparison;
            var priceComparison = Price.CompareTo(other.Price);
            if (priceComparison != 0) return priceComparison;
            var priceTextComparison = string.Compare(PriceText, other.PriceText, StringComparison.Ordinal);
            if (priceTextComparison != 0) return priceTextComparison;
            var privacyPolicyComparison = string.Compare(PrivacyPolicy, other.PrivacyPolicy, StringComparison.Ordinal);
            if (privacyPolicyComparison != 0) return privacyPolicyComparison;
            var ratingsComparison = Ratings.CompareTo(other.Ratings);
            if (ratingsComparison != 0) return ratingsComparison;
            var recentChangesComparison = string.Compare(RecentChanges, other.RecentChanges, StringComparison.Ordinal);
            if (recentChangesComparison != 0) return recentChangesComparison;
            var releasedComparison = string.Compare(Released, other.Released, StringComparison.Ordinal);
            if (releasedComparison != 0) return releasedComparison;
            var reviewsComparison = Reviews.CompareTo(other.Reviews);
            if (reviewsComparison != 0) return reviewsComparison;
            var scoreComparison = Score.CompareTo(other.Score);
            if (scoreComparison != 0) return scoreComparison;
            var scoreTextComparison = string.Compare(ScoreText, other.ScoreText, StringComparison.Ordinal);
            if (scoreTextComparison != 0) return scoreTextComparison;
            var sizeComparison = string.Compare(Size, other.Size, StringComparison.Ordinal);
            if (sizeComparison != 0) return sizeComparison;
            var summaryComparison = string.Compare(Summary, other.Summary, StringComparison.Ordinal);
            if (summaryComparison != 0) return summaryComparison;
            var titleComparison = string.Compare(Title, other.Title, StringComparison.Ordinal);
            if (titleComparison != 0) return titleComparison;
            var updatedComparison = Updated.CompareTo(other.Updated);
            if (updatedComparison != 0) return updatedComparison;
            var urlComparison = string.Compare(Url, other.Url, StringComparison.Ordinal);
            if (urlComparison != 0) return urlComparison;
            var versionComparison = string.Compare(Version, other.Version, StringComparison.Ordinal);
            if (versionComparison != 0) return versionComparison;
            var videoComparison = string.Compare(Video, other.Video, StringComparison.Ordinal);
            if (videoComparison != 0) return videoComparison;
            return string.Compare(VideoImage, other.VideoImage, StringComparison.Ordinal);
        }
    }
}
