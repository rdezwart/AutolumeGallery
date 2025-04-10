# AutolumeGallery

*An interactive virtual gallery space where users can manipulate and explore the latent space of a GAN in 3D.*

This repository is part of my project, **Exploring GAN Latent Space with Virtual Reality**. The latest build of the
project can be found as a [release](https://github.com/rdezwart/AutolumeGallery/releases/latest)!

Demo video:

[![Demo Video](http://i.ytimg.com/vi/w-MfGlRWaFY/hqdefault.jpg)](https://www.youtube.com/watch?v=w-MfGlRWaFY)

## Installation

The `AutolumeGallery` folder contains the Unity executable for this project! It should automatically connect to Autolume
once you've loaded the preset and started the OSC server, but please reach out to me if it doesn't!

The `AutolumeModel` folder contains the files and folders you should drop into your Autolume installation folder, I was
using `v2.11`.

### Troubleshooting

I've come across a few Autolume issues of note:

- The latent seed is intended to receive float values for smooth transitions, but sometimes it won't realize that unless
  the seed is manually dragged to a decimal value. After that, it should work fine as long as you don't manually change
  the seed back to an integer!
- I've had issues when loading presets, as they sometimes don't properly connect the OSC-enable features. If this is the
  case, go into Autolume and reset each field by deleting a character, putting it back, then hitting enter. This should
  fix it!
