﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    public class Algorithm
    {
        public IList<Photo> Photos { get; set; }
        public IList<Slide> TotalSlides { get; set; }

        public SlideShow Execute()
        {
            var slideShow = new SlideShow();
            JoinPhotos();
            slideShow = ProcessSlideShow();

            return slideShow;
        }

        private void JoinPhotos()
        {
            while(Photos.Count > 1)
            {
                var rngesus = new Random(DateTime.Now.Millisecond);
                var firstPhoto = Photos[rngesus.Next() % Photos.Count];
                var secondPhoto = Photos[rngesus.Next() % Photos.Count];

                TotalSlides.Add(new Slide(firstPhoto, secondPhoto));

                Photos.Remove(firstPhoto);
                Photos.Remove(secondPhoto);
            }

            TotalSlides = TotalSlides.OrderBy(x => x.Tags.Count).ToList();
        }

        private SlideShow ProcessSlideShow()
        {
            var slideShow = new SlideShow();

            var selectedSlide = TotalSlides.FirstOrDefault();
            var correlationIndex = Math.Floor(Math.Sqrt(TotalSlides.LastOrDefault()?.Tags.Count ?? 2));

            while(selectedSlide != null && TotalSlides.Count > 0)
            {
                var lowerLimit = selectedSlide.Tags.Count() - correlationIndex;
                var upperLimit = selectedSlide.Tags.Count() + correlationIndex;

                slideShow.SlidesTransition.Enqueue(selectedSlide);
                TotalSlides.Remove(selectedSlide);

                selectedSlide = TotalSlides.FirstOrDefault(slide => slide.Tags.Union(selectedSlide.Tags).Count() > 0 &&
                                                                    (slide.Tags.Count() > lowerLimit && slide.Tags.Count() < upperLimit) );
            }

            return slideShow;
        }
    }
}