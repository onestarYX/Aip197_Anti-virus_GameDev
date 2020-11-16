Documentation: http://www.niugnepsoftware.com/ngraph/
Support: graph-master@niugnepsoftware.com

=== YOU ARE READY NOW ===
If you are using the native Unity GUI (called uGUI) in Unity 4.6 or later, you are ready to go!
Make sure you have a UI Canvas in your scene and then use the Graph Wizard located under Window->Graph Master->New Native Unity Graph.

===Versions===
--- v3.0.1 ---
[FIX] Fixed mallrotated world-space UIs.  Rotation of programatically generated objects in graph hierarchy are now set to zero.
[FIX] Fixed build error when building to certain devices.

--- v3.0.0 ---
[BREAKING CHANGE]
 - A major rework of the entire asset has taken place. DO NOT UPGRADE if you are using NGUI, 2DTK, or Daikon Forge.
 - You will be forced to delete and recreate all your graphs if you update to or past this version.
 - Some small code changes will be needed as well.
   - Use GraphMaster.UGuiGraph and GraphMaster.UGuiDataSeriesXy in your code.
 - All Classes have been moved into a GraphMaster namespace to help segregate class names and prevent collisions.
 - There are zip files with older versions of Graph Master if you wish to use NGUI, 2dTK, or DaikonForge you can copy the version you want to use and delete the GraphMaster folder after unzipping the version you wish to use into the Assets area of your project.  For versions under 3.0.0, the old version will be located in the NGraph folder created from unzipping the older version.
[ENHANCEMENT] Huge performance gains.
[ENHANCEMENT] Graph now fully renders in edit mode as seen in play mode.

--- v2.0.7 ---
[FIX] Fixed assertion from Unity caused by zero width/height meshes.
[FIX] uGUI Graphs now use expanded RectTransforms.
[FIX] uGUI Graphs now correctly set clipping bounds for plots when in tested transforms.
[FIX] Transforming graph causes force redraw of graph.
[FIX] Removed errant line that was being drawn by one of the sub-canvases.
[ADD] Option on graph to turn force graph redraw on translate added: RedrawIfTranslated (default = true).
[FIX] Use Time.unscaledDeltaTime when updating plots so that it is not effected by global time scaling.

--- v2.0.6 ---
[FIX] New errors in Unity3D v5.6.0 that were emitted in test scene 01 have been fixed.

--- v2.0.5 ---
[FIX] Fixed bug with Unity v5.6.0.

--- v2.0.4 ---
[CHECK] Validated working in Unity3D v5.4.
[UPDATE] Improved performance when resolution changes.
[FIX] Fixed "walking" bug that caused graph to travel when resolution changed in certain configurations.

--- v2.0.3 ---
[CHECK] Validated working in Unity3D v5.0.

--- v2.0.2 ---
[FIX]Fixed a bug in later versions of Unit 4.6.x that was causing entire graphs not to draw.

--- v2.0.1 ---
[FIX] Removed dependencies on NGUI from main code. Users still have the option of using NGUI, but NGUI is no longer required to build native.

---2.0.0---
[NEW] Renamed asset to "Graph Master".
[NEW] uGUI Supported!  The new native Unity 4.6 GUI is now supported out-of-the-box.
[NEW] All margins (bottom, left, top, and right) are now configurable in size.
[FIX] Some components of the graph would draw in the wrong order after a re-size or other change in properties. Drawing order is now preserved.
[FIX] Reveal option on plots now finish correctly and show entire plot.
[FIX] Markers now behave correctly with reveal option.
[UPDATE] Width and Height options now available in the creation dialog.
[CHECK] NGUI v3.7.5 confirmed to work with this release.
[CHECK] Daikon Forge v1.0.16 hf1 confirmed to work with this release.
[CHECK] 2D Toolkit v2.5 confirmed to work with this release.
[NOTE] This will the the last update with Daikon Forge support.  The Daikon Forge team has been discontinued their asset, so we will no longer support it.

---1.3.0---
[NEW] Grid lines!  You can now effect grid lines via the custom Inspector or programatically.
      Options exist for:
      - Grid line separation (in both the x and y directions)
      - Grid line thickness
      - Grid line color

---1.2.2---
[FIX] NGUI v3.5.3 compatibility fixes.
[FIX] When Window size changes, graphs now behave correctly.

---1.2.1---
[FIX] Daikon Forge callbacks now use correct types in callbacks.
[FIX] Daikon Forge panels that have their enabled or visible property set will now correctly affect the graph.
[NEW] Data labels can now be added to any series using "addDataLabel(...)".

---1.2.0---
[FIX] Plots now constrained to plot area (values lower and higher than the plot area will not draw).
[NEW] Equations!!! An Equation Plot type has been added.  Most functions are sported - sin, cos, tan, log, ln, PI, E, etc.
[NEW] 2D Tool Kit GUI system support.
[NEW] Reveal speed added.
[NEW] 2DTK, NGUI and Daikon Forge graph types all have sported editor customizations - this makes it much easier to change the look and feel of the graph from the editor.

---1.1.2---
[FIX] Script errors when building for stand alone have been fixed.
[FIX] Labels in Daikon Forge (v1.0.13 f2) drawing in correct spots now.

---1.1.1---
[NEW] Daikon Forge support!
[NEW] Bar Plots!
[NEW] Added ability to define the axis location for both X and Y axes.
[NEW] Marker color can now be different than plot color.
[NEW] Added callback to allow for custom labels.  Label callback is called when a label is rendered allowing custom code to be run on label.
[NEW] NGUI - Added options for Dynamic (true type) font or bitmap font.

[FIX] NGUI 3.0.4 font axis position corrected.
[FIX] Memory leak fixed related to destroying and recreating meshes.

---1.0.1---
Initial release