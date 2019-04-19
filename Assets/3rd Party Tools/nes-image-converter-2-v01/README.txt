
                    -----------------------
                     NES Image Converter 2
                    -----------------------

                              v01

                       by thefox//aspekt
                        thefox@aspekt.fi
                     http://kkfos.aspekt.fi


General
-------

NES Image Converter 2 is a complete rewrite of my old NES Image
Converter. It allows you to convert a 256x240, 256x480 or 512x240 images
to a working NES ROM file. When the latter two image sizes are used,
the resulting ROM will flicker between two images that contain the even
and odd scanlines/columns from the source image.

Usage is self-explanatory: open an image file (or drag & drop it in),
press Convert, wait a while and save the results to a ROM file. The
image size selector can be used to select what image size the conversion
uses. If the source image has different size, it will be resized to the
specified size.

The tool doesn't perform dithering or color quantization, so for the
best results the images should be prepared in a tool like Photoshop
to reduce the number of available colors and to make sure the colors
match the NES palette as best as they can. A Photoshop palette file
(nestopia-yuv-palette.act) of the NES palette is included in the package.

For instructions on one possible way of preparing images in Photoshop
for better results:

  http://forums.nesdev.com/viewtopic.php?p=82461#p82461

For more information about this tool and its previous incarnations see
the full thread at:

  http://forums.nesdev.com/viewtopic.php?f=21&t=7363


Version History
---------------

- v01 (2013-06-29)
  * Initial release.
