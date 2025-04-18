# AutolumeGallery

*An interactive virtual gallery space where users can manipulate and explore the latent space of a GAN in 3D.*


This repository is part of my project, **Exploring GAN Latent Space with Virtual Reality**. The latest build of the
project can be found as a [release](https://github.com/rdezwart/AutolumeGallery/releases/latest)!

Watch the demo video on YouTube:

[![Demo Video](https://github.com/user-attachments/assets/41c73151-84c8-4e53-8c81-7f6f5fd8a286)](https://youtu.be/Brn7nG2WOXA)

[Original, shorter demo](https://youtu.be/w-MfGlRWaFY).

## Installation

As this is a VR based concept, you will need a headset. If you're using a **Meta Quest**, like I did, you'll want to
follow [Meta's setup guide](https://www.meta.com/ca/quest/setup/). Once you have that up and running, use the desktop
link feature to connect to your PC.

Though usable without, this project relies on [Autolume](https://www.metacreation.net/autolume/), a tool from
the [Metacreation Lab](https://www.metacreation.net/). Download Autolume, I used `v2.11`, and keep the installation
folder open.

Next, download the `AutolumeGallery` and `AutolumeModel` folders from
the [latest release](https://github.com/rdezwart/AutolumeGallery/releases/latest).

The `AutolumeModel` folder contains the GAN model, presets, and vectors for Autolume; drop these all directly into your
Autolume installation folder.

In Autolume, start `Autolume-Live` from the bottom of the screen, then scroll to the bottom of the next panel. Select
the `Voormeij` preset and press `Load`. Once it's ready, find the `Performance & OSC` section and hit `Activate Server`.

Lastly, turn your attention back to the `AutolumeGallery` folder, it contains the Unity executable. With your headset
linked to your desktop, run the executable. It should automatically connect to Autolume and you'll be good to go!

### Troubleshooting

While I've done what I can on my end, sometimes things just don't work right. If you get stuck, please feel free to
reach out to me, I'm more than happy to hop on a call and get things sorted out. :\]

During the development process, I've come across a few Autolume issues of note:

- The latent seed is intended to receive float values for smooth transitions, but sometimes it won't realize that unless
  the seed is manually dragged to a decimal value. After that, it should work fine... as long as you don't manually
  change the seed back to an integer!
- I've had issues when loading presets, as they sometimes don't properly connect the OSC-enabled features. If this is
  the case, go into Autolume and reset each field by deleting a character, putting it back, then hitting enter. This
  should fix it!
- If Autolume is working correctly, it should display a generated image when the preset is finished loading. If it
  appears to be stuck on a black screen, the best fix I've found is just closing Autolume and trying again.

My GPU and hardware is quite underpowered, so hopefully none of these should happen to you if you're using anything
recent, but I wanted to cover all my bases just in case. ;\]
