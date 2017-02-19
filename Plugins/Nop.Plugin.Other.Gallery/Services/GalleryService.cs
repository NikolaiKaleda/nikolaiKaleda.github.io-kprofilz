using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.Widgets.Gallery.Domain;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Widgets.Gallery.Services
{
    /// <summary>
    /// gallery service
    /// </summary>
    public class GalleryService : IGalleryService
    {
        #region Constants
        private const string GALLERIES_BY_ID_KEY = "Nop.gallery.id-{0}";
        private const string GALLERIES_PATTERN_KEY = "Nop.gallery.";
        private const string GALLERY_IMAGES_BY_ID_KEY = "Nop.galleryimages.id-{0}";
        private const string GALLERY_IMAGES_PATTERN_KEY = "Nop.galleryimages.";
        #endregion


        #region Fields

        private readonly IRepository<Galleries> _galleryRepository;
        private readonly IRepository<GalleryImages> _galleryImagesRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly GallerySettings _gallerySettings;
        #endregion


        #region Ctor

        public GalleryService(ICacheManager cacheManager,
            IRepository<Galleries> galleryRepository,
            IEventPublisher eventPublisher,
            IRepository<GalleryImages> galleryImagesRepository,
            GallerySettings gallerySettings)
        {
            _cacheManager = cacheManager;
            _galleryRepository = galleryRepository;
            _galleryImagesRepository = galleryImagesRepository;
            _eventPublisher = eventPublisher;
            _gallerySettings = gallerySettings;
        }
        #endregion


        #region Methods

        /// <summary>
        /// Deletes a gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        public virtual void DeleteGallery(Galleries gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");
            _galleryRepository.Delete(gallery);
            UpdateGallery(gallery);
        }

        /// <summary>
        /// Gets all gallerys
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        public virtual IList<Galleries> GetAllGalleries(bool showHidden = false)
        {
            return GetAllGalleries(null, showHidden);
        }

        /// <summary>
        /// Gets all gallerys
        /// </summary>
        /// <param name="galleryName">gallery name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        public virtual IList<Galleries> GetAllGalleries(string galleryName, bool showHidden = false)
        {
            var query = _galleryRepository.Table;
            if (!String.IsNullOrWhiteSpace(galleryName))
                query = query.Where(m => m.Name.Contains(galleryName));

            query = query.OrderBy(m => m.DisplayOrder);

            var galleries = query.ToList();
            return galleries;
        }

        /// <summary>
        /// Gets all galleries
        /// </summary>
        /// <param name="galleryName">gallery name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallerys</returns>
        public virtual IPagedList<Galleries> GetAllGalleries(string galleryName,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            var galleries = GetAllGalleries(galleryName, showHidden);
            return new PagedList<Galleries>(galleries, pageIndex, pageSize);
        }



        /// <summary>
        /// Gets a gallery
        /// </summary>
        /// <param name="galleryId">gallery identifier</param>
        /// <returns>gallery</returns>
        public virtual Galleries GetGalleryById(int galleryId)
        {
            if (galleryId == 0)
                return null;

            string key = string.Format(GALLERIES_BY_ID_KEY, galleryId);
            return _cacheManager.Get(key, () =>
            {
                var gallery = _galleryRepository.GetById(galleryId);
                return gallery;
            });
        }

        /// <summary>
        /// Inserts a gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        public virtual void InsertGallery(Galleries gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            _galleryRepository.Insert(gallery);

            //cache
            _cacheManager.RemoveByPattern(GALLERIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(gallery);
        }

        /// <summary>
        /// Updates the gallery
        /// </summary>
        /// <param name="gallery">gallery</param>
        public virtual void UpdateGallery(Galleries gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");
            _galleryRepository.Update(gallery);

            //cache
            _cacheManager.RemoveByPattern(GALLERIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(gallery);
        }

        #region gallery images


        /// <summary>
        /// Gets all gallerys
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        public virtual IList<GalleryImages> GetAllGalleryImages(bool showHidden = false)
        {
            return GetAllGalleryImages(null, showHidden);
        }

        /// <summary>
        /// Gets all gallerys
        /// </summary>
        /// <param name="galleryImageName">gallery name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallery collection</returns>
        public virtual IList<GalleryImages> GetAllGalleryImages(string galleryImageName, bool showHidden = false)
        {
            var query = _galleryImagesRepository.Table;
            if (!String.IsNullOrWhiteSpace(galleryImageName))
                query = query.Where(m => m.Name.Contains(galleryImageName));

            query = query.OrderBy(m => m.DisplayOrder);

            var galleries = query.ToList();
            return galleries;
        }


        /// <summary>
        /// Gets all galleries
        /// </summary>
        /// <param name="galleryImageName">gallery name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>gallerys</returns>
        public IPagedList<GalleryImages> GetAllGalleryImages(string galleryImageName, int pageIndex, int pageSize, bool showHidden = false)
        {
            var galleries = GetAllGalleryImages(galleryImageName, showHidden);
            return new PagedList<GalleryImages>(galleries, pageIndex, pageSize);
        }

        public GalleryImages GetGalleryImageById(int galleryId)
        {
            if (galleryId == 0)
                return null;

            string key = string.Format(GALLERY_IMAGES_BY_ID_KEY, galleryId);
            return _cacheManager.Get(key, () =>
            {
                var gallery = _galleryImagesRepository.GetById(galleryId);
                return gallery;
            });
        }


        public void InsertGalleryImage(GalleryImages gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            _galleryImagesRepository.Insert(gallery);

            //cache
            _cacheManager.RemoveByPattern(GALLERY_IMAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(gallery);
        }

        public void UpdateGalleryImage(GalleryImages gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");
            _galleryImagesRepository.Update(gallery);

            //cache
            _cacheManager.RemoveByPattern(GALLERY_IMAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(gallery);
        }

        public void DeleteGalleryImage(GalleryImages gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");
            _galleryImagesRepository.Delete(gallery);
            UpdateGalleryImage(gallery);
        }

        #endregion


        #region public info


        /// <summary>
        /// Gets gallery pages
        /// </summary>
        /// <returns>gallery collection</returns>
        public IList<Galleries> GetGalleryPages()
        {
            return GetGalleryPages(1, 9);
        }
        /// <summary>
        /// Gets gallery pages
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count">Number of images per page</param>
        /// <returns>gallery collection</returns>
        public IList<Galleries> GetGalleryPages(int page, int count)
        {
            var query = _galleryRepository.Table;
            query = query.OrderBy(g => g.DisplayOrder).Skip((page - 1) * count).Take(count);
            var gallery = query.ToList();

            return gallery;
        }


        /// <summary>
        /// Get gallery counter
        /// </summary>
        /// <returns>counter</returns>
        public double Counter(int count)
        {
            double counter = _galleryRepository.Table.Count();
            counter = counter / count;
            return counter;
        }



        public IList<GalleryImages> GetGalleryImagesPagesByGalleryId(int id, int page, int count)
        {
            var images = _galleryImagesRepository.Table.ToList();

            return _gallerySettings.SortByCreatedDate
                       ? (from image in images from i in image.Galleries where i.Id == id select image).OrderByDescending(
                           g => g.CreatedOnUtc).Skip((page - 1)*count).Take(count).ToList()
                       : (from image in images from i in image.Galleries where i.Id == id select image).OrderBy(
                           g => g.DisplayOrder).Skip((page - 1)*count).Take(count).ToList();
        }

        public IList<GalleryImages> GetGalleryImagesByGalleryId(int id)
        {
            var images = _galleryImagesRepository.Table.ToList();
            return _gallerySettings.SortByCreatedDate
                       ? (from image in images from i in image.Galleries where i.Id == id select image).OrderByDescending(
                           g => g.CreatedOnUtc).ToList()
                       : (from image in images from i in image.Galleries where i.Id == id select image).OrderBy(
                           g => g.DisplayOrder).ToList();
        }

        public IList<GalleryImages> GetImagesPagesWithoutGallery(int page, int count)
        {
            var images = _galleryImagesRepository.Table.ToList();
            return _gallerySettings.SortByCreatedDate
                       ? images.Where(image => image.Galleries.Count == 0)
                               .OrderByDescending(g => g.CreatedOnUtc)
                               .Skip((page - 1)*count)
                               .Take(count)
                               .ToList()
                       : images.Where(image => image.Galleries.Count == 0)
                               .OrderBy(g => g.DisplayOrder)
                               .Skip((page - 1)*count)
                               .Take(count)
                               .ToList();
        }


        public IList<GalleryImages> GetGalleryImagesPages(int page, int count)
        {
            var query = _galleryImagesRepository.Table;
            return _gallerySettings.SortByCreatedDate ? query.OrderByDescending(g => g.CreatedOnUtc).Skip((page - 1) * count).Take(count).ToList() :
                query.OrderBy(g => g.DisplayOrder).Skip((page - 1) * count).Take(count).ToList();
        }

        public IList<GalleryImages> GetGalleryImagesPages(int count)
        {
            return GetGalleryImagesPages(1, count);
        }

        public double ImagesCounter(int count)
        {
            double counter = _galleryImagesRepository.Table.Count();
            counter = counter / count;
            return counter;
        }

        public double ImagesCounter(int count,int id)
        {
            var images = _galleryImagesRepository.Table.ToList();
            double counter= (from image in images from i in image.Galleries where i.Id == id select image).Count();
            counter = counter / count;
            return counter;
        }

        public double ImagesWithoutGalleryCounter(int count)
        {
            var images = _galleryImagesRepository.Table.ToList();
            double counter = images.Count(image => image.Galleries.Count == 0);
            counter = counter / count;
            return counter;
        }

        #endregion



        #endregion

    }
}
