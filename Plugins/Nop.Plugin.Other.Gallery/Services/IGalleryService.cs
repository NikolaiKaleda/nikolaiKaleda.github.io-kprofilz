using Nop.Core;
using Nop.Plugin.Widgets.Gallery.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.Gallery.Services
{
    /// <summary>
    /// gallery service
    /// </summary>
    public interface IGalleryService
    {
        /// <summary>
        /// Deletes a gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        void DeleteGallery(Galleries gallery);

        /// <summary>
        /// Gets all gallerys
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        IList<Galleries> GetAllGalleries(bool showHidden = false);

        /// <summary>
        /// Gets all gallerys
        /// </summary>
        /// <param name="galleryName">gallery name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        IList<Galleries> GetAllGalleries(string galleryName, bool showHidden = false);


        /// <summary>
        /// Gets gallery pages
        /// </summary>
        /// <param name="page">current page</param>
        /// <param name="count">page count</param>
        /// <returns>gallery collection</returns>
        IList<Galleries> GetGalleryPages(int page, int count);


        /// <summary>
        /// Get all gallery
        /// </summary>
        /// <returns></returns>
        IList<Galleries> GetGalleryPages();

        /// <summary>
        /// Get counter
        /// </summary>
        /// <returns>counter</returns>
        double Counter(int count);


        /// <summary>
        /// Gets gallery pages
        /// </summary>
        /// <param name="page">current page</param>
        /// <param name="count">page count</param>
        /// <returns>gallery collection</returns>
        IList<GalleryImages> GetGalleryImagesPages(int page, int count);

        /// <summary>
        /// Get all gallery
        /// </summary>
        /// <returns></returns>
        IList<GalleryImages> GetGalleryImagesPages(int count);

        /// <summary>
        /// Get counter
        /// </summary>
        /// <returns>counter</returns>
        double ImagesCounter(int count);


        /// <summary>
        /// Get counter
        /// </summary>
        /// <returns>counter</returns>
        double ImagesCounter(int count, int id);

        /// <summary>
        /// Get counter for images without gallery
        /// </summary>
        /// <returns>counter</returns>
        double ImagesWithoutGalleryCounter(int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<GalleryImages> GetGalleryImagesPagesByGalleryId(int id,int page, int count);

        IList<GalleryImages> GetGalleryImagesByGalleryId(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<GalleryImages> GetImagesPagesWithoutGallery(int page, int count);


        /// <summary>
        /// Gets all gallerys
        /// </summary>
        /// <param name="galleryName">gallery name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallerys</returns>
        IPagedList<Galleries> GetAllGalleries(string galleryName,
            int pageIndex, int pageSize, bool showHidden = false);

        /// <summary>
        /// Gets a gallery
        /// </summary>
        /// <param name="galleryId">gallery identifier</param>
        /// <returns>gallery</returns>
        Galleries GetGalleryById(int galleryId);

        /// <summary>
        /// Inserts a gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        void InsertGallery(Galleries gallery);

        /// <summary>
        /// Updates the gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        void UpdateGallery(Galleries gallery);



        /// <summary>
        /// Gets all gallery images
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        IList<GalleryImages> GetAllGalleryImages(bool showHidden = false);

        /// <summary>
        /// Gets all gallery images
        /// </summary>
        /// <param name="galleryImageName">gallery name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        IList<GalleryImages> GetAllGalleryImages(string galleryImageName, bool showHidden = false);

        /// <summary>
        /// Gets all gallery images
        /// </summary>
        /// <param name="galleryImageName">gallery image name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallerys</returns>
        IPagedList<GalleryImages> GetAllGalleryImages(string galleryImageName,
            int pageIndex, int pageSize, bool showHidden = false);

        /// <summary>
        /// Gets gallery image
        /// </summary>
        /// <param name="galleryId">gallery identifier</param>
        /// <returns>gallery</returns>
        GalleryImages GetGalleryImageById(int galleryId);

        /// <summary>
        /// Inserts a gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        void InsertGalleryImage(GalleryImages gallery);


        /// <summary>
        /// Updates the gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        void UpdateGalleryImage(GalleryImages gallery);


        /// <summary>
        /// Deletes a gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        void DeleteGalleryImage(GalleryImages gallery);

    }
}
