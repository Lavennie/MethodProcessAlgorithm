var HandleIO = {
    WindowAlert : function(message)
    {
        window.alert(Pointer_stringify(message));
    },
    SyncFiles : function()
    {
        FS.syncfs(false,function (err) {
           if (err) console.log("syncfs error: " + err);
        });
    }
};

mergeInto(LibraryManager.library, HandleIO);