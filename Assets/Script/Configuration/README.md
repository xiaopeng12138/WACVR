## How to add configuration widget

- First, you will find the widget prefab in the ``/Asset/Prefab`` folder.
- Add any type of widget to the Config Canvans. Just follow other widgets' placement.
- You can also duplicate the existing widget.
- Rename the widget. This is very important because everything is based on the widget's name. You can have space in your widget's name but it will automatically be removed in the script. The space is for visual only since JSON does not support key names with space.
- Then you can use unity's ``onvalueChanged`` function in the inspector to control your function or make your own script.
- For scrip, pls check the ``CaptureSettingManager.cs``, you need to copy and paste the StartUp content and modify it with your need.
- First use ``var someWidget = ConfigManager.GetConfigPanelWidget("WidgetNameWithOutSpace");`` to get Widget
- Then use ``var someDropdown = someWidget.GetComponent<TMP_Dropdown>();`` to get the ui component.
- Then you can use ``someDropdown.onValueChanged.AddListener(     (int value) => {var somethingYouWantToDoWith = value}   );`` to add something you want to execute when the config is updated.
- Now you need to add the widget name to Config.cs file. This file is the default config. (This step will not be necessary in the future)