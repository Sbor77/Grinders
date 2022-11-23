var DeviceInfo = {
	IsMobilePlatform: function()
	{
		return Module.SystemInfo.mobile;
	}
};

mergeInto(LibraryManager.library, DeviceInfo);