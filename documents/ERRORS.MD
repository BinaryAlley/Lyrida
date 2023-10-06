<span style="color:#FF0000">**Problem**:</span> "There is already an open DataReader associated with this Command which must be closed first" - 
the sql data reader is probably accessed from different application points (maybe threads) at the same
time. It needs to be separated on each instance, as though it is asynchronous, it is not multi-threaded.

<span style="color:#33BB30">**Solution**</span>: change lifetime scope of modules accessing the data reader.

<span style="color:#FF0000">**Problem**:</span> SQL CONCAT doesn't work

<span style="color:#33BB30">**Solution**</span>: check if column's value is NULL, cannot be concated.