// test placeholders
const environmentTypes = [
    // ORDER OF ENVIRONMENTS MATTERS! (or, at least, their ID's must remain unchanged..)
    { id: 1, title: "Local File System", "name": "local", platformType: "Unix", initialPath: "/app/" },
    { id: 2, title: "Local File System", "name": "local", platformType: "Windows", initialPath: "C:\\Users\\Andromeda\\Desktop" },
    { id: 3, title: "File Transfer Protocol", "name": "ftp", platformType: "Unix", initialPath: "ftp://user:pasword@hostname:21/home/user" },
    { id: 4, title: "Google Drive", "name": "gdrive", platformType: "Unix", initialPath: "/test/address/" },
];

// ==================================
// Constants
// ==================================
// Development mode
const IS_DEBUG = true;
// Customizable timeout duration for scroll actions.
const SCROLL_TIMEOUT_DURATION = 1000;
// Determines the batch size for thumbnails retrieval.
const THUMBNAILS_RETRIEVAL_BATCH_SIZE = 20;
// Current icon pack theme
const CURRENT_ICON_THEME = "Lyra";
// Mapping of file extensions to SVG image paths
const FILE_ICONS = {
    "ai": "application-postscript.svg",
    "apk": "android-package-archive.svg",
    "appimage": "application-vnd.appimage.svg",
    "atom": "application-atom+xml.svg",
    "avif": "application-image.svg",
    "bmp": "application-image.svg",
    "bz": "application-x-7z-ace.svg",
    "chm": "application-vnd.ms-htmlhelp.svg",
    "dicom": "application-dicom.svg",
    "dll": "application-octet-stream.svg",
    "dot": "application-msword-template.svg",
    "doc": "application-msword-template.svg",
    "docx": "application-msword-template.svg",
    "eps": "application-postscript.svg",
    "epub": "application-epub+zip.svg",
    "exe": "application-octet-stream.svg",
    "flac": "application-ogg.svg",
    "gif": "application-image.svg",
    "gz": "application-x-7z-ace.svg",
    "ico": "application-image.svg",
    "infopath": "application-vnd.ms-infopath.svg",
    "jpeg": "application-image.svg",
    "jpg": "application-image.svg",
    "json": "application-json.svg",
    "mdb": "application-vnd.ms-access.svg",
    "mp3": "application-ogg.svg",
    "ogg": "application-ogg.svg",
    "otp": "application-vnd.oasis.opendocument.presentation-template.svg",
    "ods": "application-vnd.oasis.opendocument.spreadsheet-template.svg",
    "ots": "application-vnd.oasis.opendocument.spreadsheet-template.svg",
    "ott": "application-vnd.oasis.opendocument.text-template.svg",
    "otw": "application-vnd.oasis.opendocument.web-template.svg",
    "p7s": "application-pgp-signature.svg",
    "pdf": "application-pdf.svg",
    "pict": "application-image.svg",
    "pgp": "application-pgp.svg",
    "png": "application-image.svg",
    "ppt": "application-vnd.ms-powerpoint.svg",
    "ps": "application-postscript.svg",
    "psd": "application-photoshop.svg",
    "pub": "application-vnd.ms-publisher.svg",
    "rar": "application-x-7z-ace.svg",
    "ref": "application-vnd.flatpak.ref.svg",
    "sendfile": "application-vnd.kde.bluedevil-sendfile.svg",
    "snap": "application-vnd.snap.svg",
    "stream": "application-octet-stream.svg",
    "svg": "application-image.svg",
    "sql": "application-sql.svg",
    "sqlite": "application-sql.svg",
    "tar": "application-x-7z-ace.svg",
    "tga": "application-image.svg",
    "tiff": "application-image.svg",
    "txt": "application-pgp-signature.svg",
    "wav": "application-ogg.svg",
    "webp": "application-image.svg",
    "xdgapp": "application-vnd.xdgapp.svg",
    "xls": "application-vnd.ms-excel.svg",
    "xlsx": "application-vnd.ms-excel.svg",
    "xml": "application-atom+xml.svg",
    "zip": "application-x-7z-ace.svg"
};

// ==================================
// DOM Elements
// ==================================
const btnSelection = document.getElementById('btnSelection');
const accountIcon = document.getElementById('accountIcon');
const accountDropdown = document.getElementById('accountDropdown');
const selectionDropdown = document.getElementById('selectionDropdown');
const addressBarInput = document.getElementById('addressBarInput');
const addressBar = document.getElementById('addressBar');
const pathSegmentsContainer = document.getElementById('pathSegments');
const environmentDropdown = document.getElementById('environmentsDropdown');
const navigatorContainer = document.getElementById('navigator');
const addressBarGroup = document.getElementById('addressBarGroup');
const environmentsDropdownToggle = document.getElementById('environmentsDropdownToggle');
const environmentsCombobox = document.getElementById('environmentsCombobox');
const btnTreeView = document.getElementById('btnTreeView');
const btnPreview = document.getElementById('btnPreview');
const btnSplitView = document.getElementById('btnSplitView');
const btnNavigate = document.getElementById('btnNavigate');
const btnEditPath = document.getElementById('btnEditPath');
const btnUpOneLevel = document.getElementById('btnUpOneLevel');
const visibleSelectionRectangle = document.getElementById('visibleSelectionRectangle');
const selectionRectangle = document.getElementById('selectionRectangle');
const scrollbarHeight = getScrollbarHeight();
const scrollbarWidth = getScrollbarWidth();

/**
 * Retrieves the active's tab explorer container element.
 * @returns {HTMLElement} - The explorer container element.
 */
function getActiveExplorerContainer() {
    return document.getElementById(`explorerContainer${activeTabId}`);
}

/**
 * Retrieves the active tab's treeview container element.
 * @returns {HTMLElement} - The treeview container element.
 */
function getActiveTreeview() {
    return document.getElementById(`drivesContainer${activeTabId}`);
}

/**
 * Retrieves the active's tab preview container element.
 * @returns {HTMLElement} - The preview container element.
 */
function getActivePreview() {
    return document.getElementById(`previewContainer${activeTabId}`);
}

/**
 * Retrieves the active's tab details header element.
 * @returns {HTMLElement} - The active details header element.
 */
function getActiveDetailsHeader() {
    return document.getElementById(`detailsHeader${activeTabId}`);
}

/**
 * Retrieves the active explorer element.
 * @returns {HTMLElement} - The active explorer element.
 */
function getActiveExplorer() {
    return document.getElementById(`explorer${activeTabId}`);
}

// ==================================
// Variables
// ==================================

// these variables are declared inside FileSystem/Index!
if (typeof window.showThumbnails === "undefined")
    window.showThumbnails = true; // whether to show thumbnails or not
if (typeof window.imagePreviewsQuality === "undefined")
    window.imagePreviewsQuality = 70; // the quality used for image thumbnails
if (typeof window.fullImageQuality === "undefined")
    window.fullImageQuality = 90; // the quality used for full images
if (typeof window.rememberOpenTabs === "undefined")
    window.rememberOpenTabs = true; // whether to keep track of open tabs or not

let scrollTimeout; // timeout to delay thumbnail loading after scrolling
let resizeTimeout; // timeout to delay thumbnail loading after resizing
let hasScrolledAfterModeChange = false; // flag to track if scrolling occurred after a mode change
let abortController; // allows the cancellation of thumbnail retrieval jobs
let activeTabId; // tracks the currently active tab's ID
let activeEnvironmentId = 0; // tracks the currently active platform ID, used for creating a new tab
let addressBarWidth;
let addressBarCachedScroll = 0;
let selectionStartPosition = { x: 0, y: 0 };
let isSelecting = false;
let currentMousePosition = { x: 0, y: 0 };
let initialPages = []; // TODO: rename to openTabs
window.Permissions = [];

// ==================================
// Event Handlers
// ==================================

/**
 * Binds event listeners to the active explorer and its container.
 */
function bindEventsToActiveExplorer() {
    const explorer = getActiveExplorer();
    const explorerContainer = getActiveExplorerContainer();
    // remove any previously bound event listeners
    unbindEventsFromExplorer(explorer, explorerContainer);
    explorer.addEventListener('scroll', handleScrollEvent);
    explorerContainer.addEventListener('scroll', handleScrollEvent);
    explorer.addEventListener('wheel', scrollHorizontally, { passive: false });
    if (IS_DEBUG)
        console.info(getCurrentTime() + " Bound events to explorer with Id " + activeTabId);
}

/**
 * Unbinds event listeners from the explorer and its container.
 * @param {HTMLElement} explorer - The explorer element.
 * @param {HTMLElement} explorerContainer - The explorer container element.
 */
function unbindEventsFromExplorer(explorer, explorerContainer) {
    explorer.removeEventListener('scroll', handleScrollEvent);
    explorerContainer.removeEventListener('scroll', handleScrollEvent);
    explorer.removeEventListener('wheel', scrollHorizontally, { passive: false });
    if (IS_DEBUG)
        console.info(getCurrentTime() + " Unbound events from explorer with Id " + activeTabId);
}

/**
 * Handles the scroll event for the explorer or explorerContainer.
 * Clears any previous timeout and creates a new one to fetch visible items.
 */
function handleScrollEvent() {
    // clear any previously set timeouts to debounce the getVisibleItems call
    if (scrollTimeout) clearTimeout(scrollTimeout);
    // if there's an ongoing operation, abort it
    if (abortController) {
        abortController.abort();
        abortController = null;
    }
    // set a timeout to call getVisibleItems once the user stops scrolling
    scrollTimeout = setTimeout(() => {
        if (showThumbnails)
            getVisibleItems();
    }, SCROLL_TIMEOUT_DURATION);
    hasScrolledAfterModeChange = true;
}

/**
 * Handles the window's resize event.
 * Clears any previous timeout and creates a new one to fetch visible items.
 */
function handleResizeEvent() {
    // clear any previously set timeouts to debounce the getVisibleItems call during resize
    if (resizeTimeout) clearTimeout(resizeTimeout);
    // if there's an ongoing operation, abort it
    if (abortController) {
        abortController.abort();
        abortController = null;
    }
    // set a timeout to call getVisibleItems once the user stops resizing
    resizeTimeout = setTimeout(() => {
        if (showThumbnails)
            getVisibleItems();
    }, SCROLL_TIMEOUT_DURATION);
}

/**
 * Allows horizontal scrolling in the explorer using the mouse wheel.
 * @param {Event} event - The wheel event.
 */
function scrollHorizontally(event) {
    event.preventDefault();
    getActiveExplorer().scrollLeft += (event.deltaY > 0 ? 1 : -1) * 80;
}

/** 
 * Make address bar horizontally scrollable.
 */
if (addressBar)
    addressBar.addEventListener('wheel', function (event) {
        event.preventDefault();
        this.scrollLeft += (event.deltaY > 0 ? 1 : -1) * 80;
    }, { passive: false });

/**
 * Toggles the visibility of the folders treeview panel.
 */
if (btnTreeView)
    btnTreeView.addEventListener('click', function () {
        const treeview = getActiveTreeview();
        const computedStyle = window.getComputedStyle(treeview);
        // using computedStyle to check the actual styles being applied to the element
        treeview.style.width = computedStyle.width === '0px' ? '100px' : '0px';
        treeview.style.visibility = computedStyle.visibility === 'hidden' ? 'visible' : 'hidden';
    });

/**
 * Toggles the visibility of the file preview panel.
 */
if (btnPreview)
    btnPreview.addEventListener('click', function () {
        const preview = getActivePreview();
        const computedStyle = window.getComputedStyle(preview);
        // using computedStyle to check the actual styles being applied to the element
        preview.style.width = computedStyle.width === '0px' ? '150px' : '0px';
        preview.style.visibility = computedStyle.visibility === 'hidden' ? 'visible' : 'hidden';
    });

/**
 * Toggles the split mode for the current tab
 */
if (btnSplitView)
    btnSplitView.addEventListener('click', function () {

    });

/**
* Event handler for the navigate button click.
*/
if (btnNavigate)
    btnNavigate.addEventListener('click', function () {
        parsePath(true);
        addressBarInput.style.display = 'none';
        addressBar.style.display = 'block';
    });

/**
 * Event handler for the edit path button click.
 */
if (btnEditPath)
    btnEditPath.addEventListener('click', function () {
        addressBarInput.style.display = 'block';
        addressBarInput.focus();
        addressBar.style.display = 'none';
    });

/**
 * Event handler for the navigate up one level button click.
 */
if (btnUpOneLevel)
    btnUpOneLevel.addEventListener('click', function () {
        goUpOneLevel();
    });

/**
 * Event handler for the account icon click.
 */
if (accountIcon)
    accountIcon.addEventListener('click', function (event) {
        if (accountDropdown.classList.contains('hidden'))
            accountDropdown.classList.remove('hidden');
        else
            accountDropdown.classList.add('hidden');
        event.stopPropagation(); // this is important to prevent the document click event from hiding it immediately
    });

/**
 * Event handler for the selection icon click.
 */
if (btnSelection)
    btnSelection.addEventListener('click', function (event) {
        if (selectionDropdown.classList.contains('hidden'))
            selectionDropdown.classList.remove('hidden');
        else
            selectionDropdown.classList.add('hidden');
        event.stopPropagation(); // this is important to prevent the document click event from hiding it immediately
    });

/**
 * Event handler for the addressbar input keypress events.
 */
if (addressBarInput)
    addressBarInput.addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {  // 13 is the key code for "enter"
            event.preventDefault();  // prevent any default action
            parsePath(true);
        }
    });

/**
 * Event handler for the addressbar input keydown events.
 */
if (addressBarInput)
    addressBarInput.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {
            event.preventDefault();  // prevent any default action
            addressBarInput.style.display = 'none'; // Hide #addressBarInput
            addressBar.style.display = 'block'; // Show #addressBar
        }
    });

/**
 * Event handler for the addressbar click events.
 */
if (addressBar)
    addressBar.addEventListener('click', function (event) {
        // if the clicked element is the ul itself or one of its direct children (but not deeper nested children)
        if (event.target === event.currentTarget || event.target.parentElement === event.currentTarget) {
            this.style.display = 'none'; // hide #addressBar
            addressBarInput.style.display = 'block'; // show #addressBarInput
            addressBarInput.focus(); // focus on the input
        }
    });

// ==================================
// Event Listeners Initializations
// ==================================

// adjust thumbnails upon window resizing
window.addEventListener('resize', handleResizeEvent);
if (environmentsDropdownToggle)
    environmentsDropdownToggle.addEventListener('change', handleEnvironemtComboboxChange);

// button click handlers for different view modes.
$('#detailsView').click(() => switchViewMode(setDetailsViewMode));
$('#listView').click(() => switchViewMode(setListViewMode));
$('#smallIconsView').click(() => switchViewMode(() => setIconsViewMode('small')));
$('#mediumIconsView').click(() => switchViewMode(() => setIconsViewMode('medium')));
$('#largeIconsView').click(() => switchViewMode(() => setIconsViewMode('large')));
$('#extraLargeIconsView').click(() => switchViewMode(() => setIconsViewMode('extra-large')));

// ==================================
// Functions
// ==================================

/**
 * Gets the current time
 * @returns the current time
 */
function getCurrentTime() {
    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');
    const milliseconds = String(now.getMilliseconds()).padStart(3, '0');
    return `${hours}:${minutes}:${seconds}:${milliseconds}`;
}

/**
 * Switches to a different view mode based on a given callback.
 * @param {Function} callback - The function to switch to a specific view mode.
 */
function switchViewMode(callback) {
    hasScrolledAfterModeChange = false; // reset the flag indicating a mode change
    callback(); // execute the view mode switch
    // check if content has been scrolled after the mode change 
    setTimeout(() => {
        if (!hasScrolledAfterModeChange && showThumbnails) getVisibleItems();
    }, 50); // wait 50ms to ensure all other events have processed
}

/**
 * Sets the explorer to 'Details' view mode, displaying extended item details.
 */
function setDetailsViewMode() {
    const explorer = getActiveExplorer();
    const explorerContainer = getActiveExplorerContainer();
    const detailsHeader = getActiveDetailsHeader();
    if (explorer) {
        // update DOM elements to reflect 'Details' view mode
        explorer.classList.remove('List');
        explorer.classList.add('Details');
        explorer.scrollLeft = 0;
        explorer.style.flexDirection = "column";
        explorerContainer.classList.remove('scrollHorizontal');
        explorerContainer.classList.add('scrollVertical');
        detailsHeader.style.height = "20px";
        explorer.removeEventListener('wheel', scrollHorizontally, { passive: false });
        // update item icons to match the 'Details' view style
        explorer.querySelectorAll('.e').forEach(el => {
            el.classList.remove('list-icons', 'small-icons', 'medium-icons', 'large-icons', 'extra-large-icons');
            el.classList.add('details-icons');
        });
        // display the extended item details
        showExtraDetails();
        if (IS_DEBUG)
            console.info(getCurrentTime() + " Set Details View mode to explorer with Id " + activeTabId);
    }
}

/**
 * Sets the explorer to 'List' view mode, displaying items in a list without extended details.
 */
function setListViewMode() {
    const explorer = getActiveExplorer();
    const explorerContainer = getActiveExplorerContainer();
    const detailsHeader = getActiveDetailsHeader();
    if (explorer) {
        // update DOM elements to reflect 'List' view mode
        explorer.classList.remove('Details');
        explorer.classList.add('List');
        explorer.style.flexDirection = "column";
        explorerContainer.classList.remove('scrollVertical');
        explorerContainer.classList.add('scrollHorizontal');
        detailsHeader.style.height = "0px";
        explorer.addEventListener('wheel', scrollHorizontally, { passive: false });
        // update item icons to match the 'List' view style
        explorer.querySelectorAll('.e').forEach(el => {
            el.classList.remove('details-icons', 'small-icons', 'medium-icons', 'large-icons', 'extra-large-icons');
            el.classList.add('list-icons');
        });
        // hide the extended item details
        hideExtraDetails();
        if (IS_DEBUG)
            console.info(getCurrentTime() + " Set List View mode to explorer with Id " + activeTabId);
    }
}

/**
 * Sets the explorer to 'Icons' view mode, displaying items with specific icon sizes.
 * @param {string} size - Desired icon size ('small', 'medium', 'large', 'extra-large').
 */
function setIconsViewMode(size) {
    const explorer = getActiveExplorer();
    const explorerContainer = getActiveExplorerContainer();
    const detailsHeader = getActiveDetailsHeader();
    if (explorer) {
        // update DOM elements to reflect 'Icons' view mode
        explorer.classList.remove('List');
        explorer.classList.add('Details');
        explorer.style.flexDirection = "row";
        explorerContainer.classList.remove('scrollHorizontal');
        explorerContainer.classList.add('scrollVertical');
        detailsHeader.style.height = "0px";
        explorer.removeEventListener('wheel', scrollHorizontally, { passive: false });
        // update item icons based on the specified size
        explorer.querySelectorAll('.e').forEach(el => {
            el.classList.remove('list-icons', 'details-icons', 'small-icons', 'medium-icons', 'large-icons', 'extra-large-icons');
            el.classList.add(size + '-icons');
        });
        // hide the extended item details
        hideExtraDetails();
        if (IS_DEBUG)
            console.info(getCurrentTime() + " Set " + size + " Icons View mode to explorer with Id " + activeTabId);
    }
}

/**
 * Hides additional item details such as date modified, type, and size.
 */
function hideExtraDetails() {
    document.querySelectorAll('.date-modified, .type, .size').forEach(el => {
        el.style.display = 'none';
    });
}

/**
 * Shows additional item details such as date modified, type, and size.
 */
function showExtraDetails() {
    document.querySelectorAll('.date-modified, .type, .size').forEach(el => {
        el.style.display = 'flex';
    });
}

/**
 * Fetches directory and file data for the given file system path. 
 * @param {string} path - The file system path to fetch data from.
 * @param {string} environmentId - The Id of the environment for which to fetch the data at the provided path.
 * @param {string} uuid - The UUID associated with the current explorer instance.
 * @param {string} title - The title of the tab.
 * @param {Function} callback - A function to be executed after successful data retrieval.
 */
function fetchDataForPath(path, environmentId, uuid, title, callback) {
    if (path !== null) {
        // fetch directory data for the path.
        $.ajax({
            url: baseUrl + '/FileSystem/GetDirectories?path=' + encodeURIComponent(path),
            type: 'GET',
            headers: {
                "X-Environment-Type": environmentId.toString()
            },
            success: function (directoriesData) {
                if (directoriesData.success) {
                    if (rememberOpenTabs) 
                        updatePageInStorage(uuid, path, environmentId, title);
                    // fetch file data, upon successful directory fetch.
                    $.ajax({
                        url: baseUrl + '/FileSystem/GetFiles?path=' + encodeURIComponent(path),
                        type: 'GET',
                        headers: {
                            "X-Environment-Type": environmentId.toString()
                        },
                        success: function (filesData) {
                            if (filesData.success) {
                                // combine directory and file data.
                                const combinedData = {
                                    path: path,
                                    directories: directoriesData.directories,
                                    files: filesData.files
                                };
                                callback(combinedData);
                                if (IS_DEBUG)
                                    console.info(getCurrentTime() + " Got the directories and files for: " + path);
                            }
                        },
                        error: function (error) {
                            console.error('Failed to fetch files:', error);
                        }
                    });
                }
            },
            error: function (error) {
                console.error('Failed to fetch directories:', error);
            }
        });
    } else
        callback(null);
}

/**
 * Navigates up one level from the current path
 */
function goUpOneLevel() {
    const foundEnvironment = environmentTypes.find(environment => environment.id === activeEnvironmentId);
    const foundEnvironmentPlatformType = foundEnvironment.platformType;
    const separator = (foundEnvironmentPlatformType === "Unix") ? '/' : '\\';
    if (!addressBarInput.value.endsWith(separator))
        addressBarInput.value = addressBarInput.value + separator;
    const path = addressBarInput.value;
    $.ajax({
        url: baseUrl + '/FileSystem/GoUpOneLevel?path=' + encodeURIComponent(path),
        type: 'GET',
        headers: {
            "X-Environment-Type": activeEnvironmentId.toString()
        },
        success: function (data) {
            if (data.success) {
                renderAddressBar(data.pathSegments);
                addressBarInput.style.display = 'none'; 
                addressBar.style.display = 'block'; 
                // find the last directory in the list of path segments
                var lastDirectory = null;
                for (var i = data.pathSegments.length - 1; i >= 0; i--) {
                    if (data.pathSegments[i].isDirectory) {
                        lastDirectory = data.pathSegments[i];
                        break;
                    }
                }
                const concatenatedPath = (foundEnvironmentPlatformType === "Unix" ? '/' : '') + data.pathSegments.map(segment => segment.name).join(separator);
                addressBarInput.value = concatenatedPath + (!concatenatedPath.endsWith(separator) ? separator : '');
                updateCurrentTab(concatenatedPath, lastDirectory ? lastDirectory.name : (data.pathSegments.length > 0 ? data.pathSegments[data.pathSegments.length - 1].name + separator : "New Tab"));
            }
            else
                console.error(data.errorMessage);
        },
        error: function (error) {
            console.error('Failed to fetch files:', error);
        }
    });
}

/**
 * Adds a new tab with its title and path, optionally with a specified ID.
 * Fetches directory and file data for the tab and renders its content.
 * @param {string} title - The title of the tab.
 * @param {string} path - The file path associated with the tab.
 * @param {number} platformId - The platform ID for the tab.
 * @param {string} uuid - The UUID associated with the current explorer instance.
*/
function addNewTab(title, path, platformId, uuid) {
    if (IS_DEBUG)
        console.info("======== add tab ========")
    if (platformId !== null)
        activeEnvironmentId = platformId;
    // determine the tab ID
    const currentTabId = nextTabId++;
    // construct the new page object
    const newPage = { title: title, path: path, tabId: currentTabId, platformId: activeEnvironmentId };
    initialPages.push(newPage);
    // render the new tab's header
    renderTabHeader(newPage);
    // set the current path as the title of the tab header
    document.getElementById(`tabHeader${currentTabId}`).setAttribute("title", path);
    // fetch directory data for the new tab
    fetchDataForPath(path, activeEnvironmentId, uuid, title, function (data) {
        // render the tab's content and bind events.
        renderTabPage(currentTabId, data, uuid);
        bindEventsToActiveExplorer();
        switchViewMode(setListViewMode);
    });
}

/**
 * Updates the content of the currently active tab with the data from the provided path.
 * @param {string} path - The file system path to fetch data from.
 * @param {string} title - The title of the active tab.
 */
function updateCurrentTab(path, title) {
    const explorer = getActiveExplorer();
    if (explorer) {
        // if there was a job for thumbnails retrieval, cancel it
        if (abortController) {
            abortController.abort();
            abortController = null;
        }
        // update the tab header title
        let tabHeader = document.getElementById(`tabHeader${activeTabId}`);
        explorer.setAttribute("data-path", path);
        const uuid = explorer.getAttribute("data-uuid");
        tabHeader.getElementsByTagName('a')[0].textContent = title;
        fetchDataForPath(path, activeEnvironmentId, uuid, title, function (data) {
            // update the current tab's content with the fetched data
            updateTabContent(activeTabId, data);
            // rebind necessary event handlers to the updated content
            bindEventsToActiveExplorer();
            switchViewMode(setListViewMode);
        });
        // set the current path as the title of the tab header
        document.getElementById(`tabHeader${activeTabId}`).setAttribute("title", path);
    }
    else
        addNewTab(title, path, null, null);
}

/**
 * Activates the provided tab based on the given tabId. 
 * @param {string} tabId - The ID of the tab to be activated.
 */
function switchTab(tabId) {
    // deactivate all tabs
    $('.dynTab-tabs > li').removeClass('active');
    // hide sibling tabs and show the current tab
    $(`#tabPage${tabId}`).siblings().hide();
    $(`#tabPage${tabId}`).show();
    // set the current tab as active
    $(`#tabHeader${tabId}`).addClass('active');
    const explorer = document.getElementById(`explorer${tabId}`);
    const environmentId = Number(explorer.getAttribute('data-environment'));
    const environment = environmentTypes.find(item => item.id === environmentId);
    activeEnvironmentId = environmentId;
    // set the environment drop down values (title, icon, etc)
    const environmentsCombobox = document.getElementById('environmentsCombobox');
    const imgElement = environmentsCombobox.querySelector("label.enlightenment-toggle img");
    if (typeof environment !== 'undefined') { //getEnvironmentIconPath(type);
        imgElement.src = getEnvironmentIconPath(environment);
        imgElement.setAttribute('title', (environment.title === "Local File System") ? `${environment.title} (${environment.platformType})` : environment.title);
        imgElement.setAttribute('alt', (environment.title === "Local File System") ? `${environment.title} (${environment.platformType})` : environment.title);
        environmentsCombobox.setAttribute('title', (environment.title === "Local File System") ? `${environment.title} (${environment.platformType})` : environment.title);
    }
    else {
        imgElement.setAttribute('title', imgElement.getAttribute('data-default-text'));
        imgElement.setAttribute('alt', imgElement.getAttribute('data-default-text'));
        imgElement.src = baseUrl + "/images/ui/os.svg";
        environmentsCombobox.setAttribute('title', imgElement.getAttribute('data-default-text'));
    }
    // update address bar input with current explorer path
    const path = explorer.getAttribute('data-path');
    if (path !== null && path !== "null") {
        addressBarInput.value = path;
        parsePath(false);
        environmentsDropdownToggle.checked = false; // toggle the checkbox state
    } else {
        addressBarInput.value = '';
        environmentsDropdownToggle.checked = true; // toggle the checkbox state
        renderAddressBar(null);
        console.log(activeTabId);
    }
    environmentsDropdownToggle.dispatchEvent(new Event('change'));
    // update the active tab id
    activeTabId = tabId;
    if (showThumbnails)
        getVisibleItems(); // get currently visible items in the tab
}

/**
 * Renders and initializes a new tab in the UI based on provided page data.
 * @param {Object} page - The page data for the new tab, containing at least 'id' and 'title'.
 */
function renderTabHeader(page) {
    // create tab header
    let tabHeader = document.createElement("li");
    tabHeader.id = `tabHeader${page.tabId}`;
    // create tab link
    let tabLink = document.createElement("a");
    tabLink.href = "#";
    tabLink.innerText = page.title;
    // create close button for the tab
    let closeButton = document.createElement("a");
    closeButton.href = "#";
    closeButton.className = "closeIcon";
    closeButton.title = "Close";
    closeButton.addEventListener('click', (event) => {
        event.preventDefault();
        event.stopPropagation(); // prevent tab selection post close
        // check if current tab is active
        const isActive = $(tabHeader).hasClass('active');
        // determine next and previous tabs
        const nextTab = $(tabHeader).next('li');
        const prevTab = $(tabHeader).prev('li');
        // remove the page from storage
        if (rememberOpenTabs) {
            const uuid = $(`#explorer${page.tabId}`).data('uuid');
            removePageFromStorage(uuid);
        }
        // remove current tab header and content
        tabHeader.remove();
        $(`#tabPage${page.tabId}`).remove();
        // remove tab data from initialPages array
        const index = initialPages.findIndex(p => p.tabId === page.tabId);
        if (index !== -1)
            initialPages.splice(index, 1);
        // switch to an adjacent tab if current tab was active
        if (isActive) {
            if (prevTab.length) {
                prevTab.addClass('active');
                switchTab(prevTab.attr('id').replace('tabHeader', ''));
            } else if (nextTab.length && nextTab.attr('id') !== 'dynTab_addTab') {
                nextTab.addClass('active');
                switchTab(nextTab.attr('id').replace('tabHeader', '')); 
            }
        }
        // if there are no more tabs, set the mode for selecting an environment
        if (initialPages.length === 0) {
            addressBarInput.value = '';
            renderAddressBar(null);
            const environmentsCombobox = document.getElementById('environmentsCombobox');
            const imgElement = environmentsCombobox.querySelector("label.enlightenment-toggle img");
            imgElement.setAttribute('title', imgElement.getAttribute('data-default-text'));
            imgElement.setAttribute('alt', imgElement.getAttribute('data-default-text'));
            imgElement.src = baseUrl + "/images/ui/os.svg";
            environmentsCombobox.setAttribute('title', imgElement.getAttribute('data-default-text'));
        }
    });
    // append tab link and close button to the tab header
    tabHeader.appendChild(tabLink);
    tabHeader.appendChild(closeButton);
    // switch to tab when its header is clicked
    tabHeader.addEventListener('click', (event) => {
        event.preventDefault();
        event.stopPropagation(); // prevent tab selection post close
        switchTab(page.tabId)
    });
    // create tab content container
    let tabPage = document.createElement("div");
    tabPage.id = `tabPage${page.tabId}`;
    // insert the tab header and content container into DOM
    $("#dynTab_tabHeaders").prepend(tabHeader);
    $("#dynTab_tabPages").append(tabPage);
}

/**
 * Renders the content for a given tab using provided data.
 * @param {string} tabId - The unique identifier for the tab.
 * @param {Object} data - Data containing directories and files to be rendered.
 * @param {string} uuid - The UUID associated with the current explorer instance.
*/
function renderTabPage(tabId, data, uuid) {
    // DOM Elements
    const drivesContainer = createContainer(`drivesContainer${tabId}`);
    const explorerContainer = createContainer(`explorerContainer${tabId}`, "scrollHorizontal");
    const detailsHeader = createDetailsHeader(tabId);
    const explorer = createExplorer(tabId, data !== null ? data.path : null, uuid !== null ? uuid : null);
    const previewContainer = createContainer(`previewContainer${tabId}`);
    // populate explorer with directories and files
    if (data !== null)
        populateExplorerWithData(explorer, data);
    // appending elements to the DOM
    explorerContainer.appendChild(detailsHeader);
    explorerContainer.appendChild(explorer);
    $(explorerContainer).on('mousedown', function (event) {

        const explorer = getActiveExplorer();
        if (explorer) {
            const explorerScrollbars = hasScrollbars(explorer);
            const containerExplorerScrollbars = hasScrollbars(explorerContainer);
            // check if clicked on the horizontal or vertical scrollbar
            if (explorerScrollbars.horizontal && event.clientY > $(explorer).offset().top + $(explorer).height() - scrollbarHeight) 
                return;
            if (containerExplorerScrollbars.vertical && event.clientX > $(explorerContainer).offset().left + $(explorerContainer).width() - scrollbarWidth) 
                return;
            // store information about selection start
            isSelecting = true;
            document.body.style.overflowY = 'hidden'; // needed in order to not put a vertical scrollbar on the document, while in vertical layout selection
            document.documentElement.style.overflowY = 'hidden';
            selectionStartPosition.x = event.pageX + $(explorer).scrollLeft();
            selectionStartPosition.y = event.pageY + $(explorerContainer).scrollTop(); // not a mistake - in vertical mode, this is the overflow container!
           
            visibleSelectionRectangle.style.left = event.pageX + 'px';
            visibleSelectionRectangle.style.top = event.pageY + 'px';
            visibleSelectionRectangle.style.width = '0px';
            visibleSelectionRectangle.style.height = '0px';
            visibleSelectionRectangle.style.visibility = 'visible';

            selectionRectangle.style.left = event.pageX + 'px';
            selectionRectangle.style.top = event.pageY + 'px';
            selectionRectangle.style.width = '0px';
            selectionRectangle.style.height = '0px';
        }
    });
    $(explorerContainer).on('mousemove', function (event) {
        if (!isSelecting)
            return;
        currentMousePosition.x = event.pageX;
        currentMousePosition.y = event.pageY;
        handleSelection(explorerContainer);
    });

    $(explorerContainer).on('mouseup', function (event) {        
        // selection ended, hide the selection rectangle
        isSelecting = false;
        document.body.style.removeProperty('overflow-y');
        document.documentElement.style.removeProperty('overflow-y');

        visibleSelectionRectangle.style.visibility = 'hidden';
        selectionRectangle.style.visibility = 'hidden';

        selectionRectangle.style.left = '0px';
        selectionRectangle.style.top = '0px';
        selectionRectangle.style.width = '0px';
        selectionRectangle.style.height = '0px';
        visibleSelectionRectangle.style.left = '0px';
        visibleSelectionRectangle.style.top = '0px';
        visibleSelectionRectangle.style.width = '0px';
        visibleSelectionRectangle.style.height = '0px';
        // change color of items that fall within selection rectangle
        const explorer = getActiveExplorer();
        if (explorer) {
            // go through each .e item that was inside the selection rectangle, and mark it as selected
            explorer.querySelectorAll('.e.selectionHover').forEach(item => {                
                item.classList.add('selected');
                item.classList.remove('selectionHover');
            });
        }
    });
    // both wheel and scroll functions are needed, otherwise scrollTop() and scrollLeft() values are not updated, or the event is not fired
    $(explorerContainer).on('wheel', function (event) {
        handleSelection(explorerContainer);
    });
    $(explorerContainer).on('scroll', function (event) {
        handleSelection(explorerContainer);
    });
    const tabPage = document.querySelector(`#tabPage${tabId}`);
    tabPage.appendChild(drivesContainer);
    tabPage.appendChild(explorerContainer);
    tabPage.appendChild(previewContainer);
    // switch to the newly rendered tab
    switchTab(tabId);
}

/**
 * Handles selection rectangle updates and item highlighting within the explorer container.
 * @param {HTMLElement} explorerContainer - The main container for item selection.
 */
function handleSelection(explorerContainer) {
    if (!isSelecting)
        return;
    const explorer = getActiveExplorer();
    if (explorer) {
         // calculate dimensions of the selection rectangle
        const currentX = currentMousePosition.x;
        const currentY = currentMousePosition.y;
        const width = currentX - (selectionStartPosition.x - $(explorer).scrollLeft());
        const height = currentY - (selectionStartPosition.y - $(explorerContainer).scrollTop());

        // update selection rectangle dimensions
        visibleSelectionRectangle.style.width = Math.abs(width) + 'px';
        visibleSelectionRectangle.style.height = Math.abs(height) + 'px';
        selectionRectangle.style.width = Math.abs(width) + 'px';
        selectionRectangle.style.height = Math.abs(height) + 'px';

        // adjust position based on selection direction
        if (width < 0) {
            selectionRectangle.style.left = currentX + 'px';
            visibleSelectionRectangle.style.left = currentX + 'px';
        }
        else {
            selectionRectangle.style.left = (selectionStartPosition.x - $(explorer).scrollLeft()) + 'px';
            visibleSelectionRectangle.style.left = (selectionStartPosition.x - $(explorer).scrollLeft()) + 'px';
        }
        if (height < 0) {
            selectionRectangle.style.top = currentY + 'px';
            visibleSelectionRectangle.style.top = currentY + 'px';
        }
        else {
            selectionRectangle.style.top = (selectionStartPosition.y - $(explorerContainer).scrollTop()) + 'px';
            visibleSelectionRectangle.style.top = (selectionStartPosition.y - $(explorerContainer).scrollTop()) + 'px';
        }

        // do not let the selection rectangle go outside of explorer container, clip it if needed
        const containerRect = explorerContainer.getBoundingClientRect();
        const left = parseFloat(visibleSelectionRectangle.style.left);
        const top = parseFloat(visibleSelectionRectangle.style.top) + 1;
        const right = left + parseFloat(visibleSelectionRectangle.style.width);
        const bottom = top + parseFloat(visibleSelectionRectangle.style.height);
        if (left < containerRect.left) {
            const difference = containerRect.left - left;
            visibleSelectionRectangle.style.width = (parseFloat(visibleSelectionRectangle.style.width) - difference) + 'px';
            visibleSelectionRectangle.style.left = containerRect.left + 'px';
        }
        if (right > containerRect.right) 
            visibleSelectionRectangle.style.width = (containerRect.right - left) + 'px';

        if (top < containerRect.top) {
            const difference = containerRect.top - top;
            visibleSelectionRectangle.style.height = (parseFloat(visibleSelectionRectangle.style.height) - difference) + 'px';
            visibleSelectionRectangle.style.top = containerRect.top + 'px';
        }
        if (bottom > containerRect.bottom) 
            visibleSelectionRectangle.style.height = (containerRect.bottom - top) + 'px';  

        // change color of items that fall within selection rectangle            
        const selectionRect = selectionRectangle.getBoundingClientRect();
        // go through each .e item which has not had its thumbnail retrieved yet
        explorer.querySelectorAll('.e').forEach(item => {
            const itemRect = item.getBoundingClientRect();
            // check if the item is fully or partially visible horizontally and vertically
            const isFullyVisibleHorizontally = itemRect.left >= selectionRect.left && itemRect.right <= selectionRect.right;
            const isFullyVisibleVertically = itemRect.top >= selectionRect.top && itemRect.bottom <= selectionRect.bottom;
            const isPartiallyVisibleHorizontally = itemRect.left < selectionRect.right && itemRect.right > selectionRect.left;
            const isPartiallyVisibleVertically = itemRect.top < selectionRect.bottom && itemRect.bottom > selectionRect.top;
            // if the item is fully or partially visible in both directions, add it to our list
            if ((isFullyVisibleHorizontally || isPartiallyVisibleHorizontally) &&
                (isFullyVisibleVertically || isPartiallyVisibleVertically)) {
                item.classList.add('selectionHover');
            }
            else
                item.classList.remove('selectionHover');
        });
    }
}

/**
 * Updates the contents of a tab page with the provided data.
 * @param {any} tabId - The id of the tab page for which to update the content.
 * @param {any} data - The data to be used as the content for the tab page.
 */
function updateTabContent(tabId, data) {
    const explorer = getActiveExplorer();
    while (explorer.firstChild)
        explorer.removeChild(explorer.firstChild); // clear the current content
    // populate explorer with new directories and files.
    populateExplorerWithData(explorer, data);
}

/**
 * Creates a container element with specified attributes.
 * @param {string} id - The ID to set on the container.
 * @param {string} [className=""] - The class name to set on the container. Default is an empty string.
 * @returns {HTMLElement} - Returns the created container element.
 */
function createContainer(id, className = "") {
    const container = document.createElement("div");
    container.id = id;
    if (className)
        container.className = className;
    return container;
}

/**
 * Creates the details header for the explorer.
 * @param {string} tabId - The ID associated with the current tab.
 * @returns {HTMLElement} - Returns the created details header element.
 */
function createDetailsHeader(tabId) {
    const detailsHeader = createContainer(`detailsHeader${tabId}`);
    const nameSpan = document.createElement("span");
    nameSpan.innerText = "Name";
    detailsHeader.appendChild(nameSpan);
    return detailsHeader;
}

/**
 * Creates the main explorer container for directories and files.
 * @param {string} tabId - The ID associated with the current tab.
 * @param {string} path - The data-path attribute to set on the explorer.
 * @param {string} uuid - The UUID associated with the current explorer instance.
 * @returns {HTMLElement} - Returns the created explorer container.
 */
function createExplorer(tabId, path, uuid) {
    const guid = uuid !== null ? uuid : generateUUID();
    const explorer = createContainer(`explorer${tabId}`, "List");
    explorer.setAttribute("data-path", path);
    explorer.setAttribute("data-environment", activeEnvironmentId);
    explorer.setAttribute("data-uuid", guid);
    explorer.style.flexDirection = "column"; // TODO: change based on preferences of initial display view    
    explorer.addEventListener('mouseup', (event) => {
        const explorerScrollbars = hasScrollbars(explorer);
        // check if clicked on the horizontal scrollbar
        if (explorerScrollbars.horizontal && event.clientY > $(explorer).offset().top + $(explorer).height() - scrollbarHeight)
            return;
        if (!event.ctrlKey && !event.shiftKey) {
            // deselect all file system elements when their explorer is clicked
            explorer.querySelectorAll('.e.selected').forEach(item => {
                item.classList.remove('selected');
            });
            // do not store information about what item was clicked first, in context of Shift-selection
            explorer.removeAttribute('data-selection-start');
        }
    });
    if (rememberOpenTabs && uuid === null) { // only store pages when the option is enabled and its a brand new page, not one already stored
        const lastAddedPage = initialPages[initialPages.length - 1];
        addNewPageToStorage(guid, lastAddedPage.title, lastAddedPage.path, lastAddedPage.platformId);
    }
    return explorer;
}

/**
 * Populates the explorer element with directories and files from the provided data.
 * @param {HTMLElement} explorer - The explorer element to populate.
 * @param {Object} data - Data containing directories and files.
 */
function populateExplorerWithData(explorer, data) {
    if (Array.isArray(data.directories)) {
        data.directories.forEach(directory => {
            const directoryDiv = createFileSystemEntity(directory, "directory");
            explorer.appendChild(directoryDiv);
        });
    }
    if (Array.isArray(data.files)) {
        data.files.forEach(file => {
            const fileDiv = createFileSystemEntity(file, "file");
            explorer.appendChild(fileDiv);
        });
    }
}

/**
 * Creates a DOM element for a file or directory.
 * @param {Object} entity - The file or directory object.
 * @param {string} type - Type of the entity ("file" or "directory").
 * @returns {HTMLElement} - Returns the created DOM element.
 */
function createFileSystemEntity(entity, type) {
    const entityDiv = document.createElement("div");
    entityDiv.className = "e list-icons"; // TODO: change "list-icons" with customizable property
    entityDiv.dataset.path = entity.path;
    entityDiv.dataset.type = type;    
    entityDiv.addEventListener('mouseup', handleFileSystemEntityClick);
    entityDiv.addEventListener('mousedown', function (event) {
        // stop the event from bubbling up to the explorer - we dont want to start selection rectangles when clicking files or directories
        event.stopPropagation();
    });
    // create and append child divs to the entityDiv
    entityDiv.appendChild(createIconDiv(entity.path, type));
    entityDiv.appendChild(createTextDiv(entity.name, type));
    entityDiv.appendChild(createDateModifiedDiv(entity.dateCreated, type));
    entityDiv.appendChild(createSizeDiv(entity.size, type));
    return entityDiv;
}

/**
 * Creates an icon div for a file or directory.
 * @param {string} path - The path to the file or directory.
 * @returns {HTMLElement} - Returns the created icon div.
 */
function createIconDiv(path, type) {
    const iconDiv = document.createElement("div");
    iconDiv.className = "icon";
    const fileImg = document.createElement("img");
    fileImg.src = baseUrl + "/images/icons/" + CURRENT_ICON_THEME + "/" + getIconPathForFile(path, type);
    iconDiv.appendChild(fileImg);
    return iconDiv;
}

/**
 * Creates a text div for a file or directory.
 * @param {string} name - The name of the file or directory.
 * @param {string} type - Type of the entity ("file" or "directory").
 * @returns {HTMLElement} - Returns the created text div.
 */
function createTextDiv(name, type) {
    const textDiv = document.createElement("div");
    textDiv.className = "text";
    const fileNameSpan = document.createElement("span");
    fileNameSpan.className = type === "directory" ? "d t" : "f t";
    fileNameSpan.textContent = name;
    textDiv.appendChild(fileNameSpan);
    return textDiv;
}

/**
 * Creates a date modified div for a file or directory.
 * @param {Date} dateCreated - The date the file or directory was created.
 * @param {string} type - Type of the entity ("file" or "directory").
 * @returns {HTMLElement} - Returns the created date modified div.
 */
function createDateModifiedDiv(dateCreated, type) {
    const dateModifiedDiv = document.createElement("div");
    dateModifiedDiv.className = `date-modified ${type === "directory" ? "d" : "f"}`;
    dateModifiedDiv.style.display = "none";
    dateModifiedDiv.textContent = new Date(dateCreated).toLocaleString();
    return dateModifiedDiv;
}

/**
 * Creates a size div for a file or directory.
 * @param {number|string} size - The size of the file or directory.
 * @param {string} type - Type of the entity ("file" or "directory").
 * @returns {HTMLElement} - Returns the created size div.
 */
function createSizeDiv(size, type) {
    const sizeDiv = document.createElement("div");
    sizeDiv.className = `size ${type === "directory" ? "d" : "f"}`;
    sizeDiv.style.display = "none";
    sizeDiv.textContent = size;
    return sizeDiv;
}

/**
 * Handle the logic when a file or directory is clicked.
 * @param {Event} event - The click event object.
 */
function handleFileSystemEntityClick(event) {
    event.preventDefault();
    if (!isSelecting)
        event.stopPropagation();

    const explorer = getActiveExplorer();
    const allEClassElements = Array.from(explorer.querySelectorAll('.e'));
    const currentIndex = allEClassElements.indexOf(event.currentTarget);

    // If a previous click timestamp exists, check the time difference
    const previousTime = parseInt(event.currentTarget.dataset.selectionStartTime || 0);
    const currentTime = new Date().getTime();
    const timeDifference = currentTime - previousTime;

    // if time difference is less than 300ms, treat it as a double-click
    if (timeDifference < 300) { 
        if (event.currentTarget.dataset.type === 'directory') { // directories
            if (IS_DEBUG)
                console.info(getCurrentTime() + " Double clicked: " + event.currentTarget.dataset.path);
            addressBarInput.value = event.currentTarget.dataset.path;
            parsePath(true); // TODO: add to undo 
            return;
        } else { // files

        }
        // reset the timestamp used to track double clicks
        event.currentTarget.dataset.selectionStartTime = 0;
        return;
    }

    if (event.ctrlKey) { // when CTRL key is pressed
        // toggle selection for the current clicked item
        event.currentTarget.classList.toggle('selected');
        // Set the selection start if it's not already set
        if (explorer.dataset.selectionStart === undefined) 
            explorer.dataset.selectionStart = currentIndex;      
    } else if (event.shiftKey && explorer.dataset.selectionStart !== undefined) { // when SHIFT key is pressed
        const start = parseInt(explorer.dataset.selectionStart);
        // remove all selections
        allEClassElements.forEach(elem => {
            elem.classList.remove('selected');
        });
        // define the range to select items from and to
        const [from, to] = start < currentIndex ? [start, currentIndex] : [currentIndex, start];
        // add the 'selected' class to items in the range
        for (let i = from; i <= to; i++) 
            allEClassElements[i].classList.add('selected');       
    } else { // when neither CTRL nor SHIFT keys are pressed
        // deselect all items
        allEClassElements.forEach(elem => {
            elem.classList.remove('selected');
            // remove selectionStartTime from all items
            delete elem.dataset.selectionStartTime;
        });
        // add 'selected' class to the current clicked item
        event.currentTarget.classList.add('selected');
        // set the current item as the selection start
        explorer.dataset.selectionStart = currentIndex;
        // store the current timestamp for future double-click checks
        event.currentTarget.dataset.selectionStartTime = currentTime;
    }
}

/**
 * Identifies items within the explorerContainer that are either fully or partially visible.
 * Items with thumbnails already retrieved are excluded.
 * Populates a list of such visible items and passes them for further processing.
 */
function getVisibleItems() {
    const explorer = getActiveExplorer();
    if (explorer) {
        const explorerContainer = explorer.closest('[id^="explorerContainer"]');

        const containerRect = explorerContainer.getBoundingClientRect();
        const visibleItems = [];
        // go through each .e item which has not had its thumbnail retrieved yet
        explorer.querySelectorAll('.e:not([data-thumbnail-retrieved])[data-type="file"]').forEach(item => {
            const itemRect = item.getBoundingClientRect();
            // check if the item is fully or partially visible horizontally and vertically
            const isFullyVisibleHorizontally = itemRect.left >= containerRect.left && itemRect.right <= containerRect.right;
            const isFullyVisibleVertically = itemRect.top >= containerRect.top && itemRect.bottom <= containerRect.bottom;
            const isPartiallyVisibleHorizontally = itemRect.left < containerRect.right && itemRect.right > containerRect.left;
            const isPartiallyVisibleVertically = itemRect.top < containerRect.bottom && itemRect.bottom > containerRect.top;
            // if the item is fully or partially visible in both directions, add it to our list
            if ((isFullyVisibleHorizontally || isPartiallyVisibleHorizontally) &&
                (isFullyVisibleVertically || isPartiallyVisibleVertically)) {
                const imgElement = item.querySelector('img');
                visibleItems.push({
                    path: item.getAttribute('data-path'),
                    img: imgElement
                });
            }
        });
        // process the list of visible items
        processVisibleItems(visibleItems);
    }
}

/**
 * Processes a given item by retrieving its thumbnail from an API and updating the item's thumbnail image.
 * If there are more items in the visibleItems list, it continues to process the next item.
 * If an error occurs during the retrieval process (including an aborted fetch operation), it is handled gracefully.
 * @param {Object} item - The item containing its path and corresponding image element.
 * @param {Array} visibleItems - List of remaining visible items to process.
 * @throws {AbortError} - Indicates the fetch operation was aborted.
 * @throws {Error} - Catches and logs other errors that might occur during the thumbnail retrieval.
 */
async function processItem(item, visibleItems) {
    try {
        const result = await getThumbnailApiCall(item.path);
        // update the item's thumbnail using the retrieved data
        if (typeof result.base64Data !== "undefined")
            item.img.src = `data:${result.mimeType};base64,${result.base64Data}`;
        item.img.closest('.e').setAttribute('data-thumbnail-retrieved', 'true');
        // console.log(`Finished processing item: ${item.path}`);
        // continue with next item if there are more in the queue
        if (visibleItems.length > 0) {
            const nextItem = visibleItems.shift();
            processItem(nextItem, visibleItems);
        }
    } catch (error) {
        if (error.name === 'AbortError') {
            // console.log('Fetch operation was aborted.');
        } else {
            // on error, revert the icons to the default ones
            let itemType = item.img.closest('.e').getAttribute('data-type');
            if (itemType === 'directory')
                item.img.src = baseUrl + "/images/icons/" + CURRENT_ICON_THEME + "/directory.svg";
            else if (itemType === 'file')
                item.img.src = baseUrl + "/images/icons/" + CURRENT_ICON_THEME + "/file.svg";
            console.error('Error processing item:', error);
        }
    }
}

/**
 * Makes an API call to retrieve a thumbnail for the given item.
 * @param {string} path - The path of the item for which the thumbnail needs to be fetched.
 * @returns {Promise<object>} - Returns a Promise resolving to an object containing base64Data and mimeType.
 * @throws {Error} - Throws an error if the API call fails.
 */
async function getThumbnailApiCall(path) {
    // URL encode the path to ensure it's safely transmitted in the URL
    const encodedItem = encodeURIComponent(path);
    // fetch the thumbnail for the given item from the server
    const response = await fetch(baseUrl + '/FileSystem/GetThumbnail?path=' + encodeURIComponent(path) + '&quality=' + imagePreviewsQuality, {
        headers: {
            'X-Environment-Type': activeEnvironmentId.toString()
        },
        signal: abortController.signal // use the abort signal 
    });
    if (!response.ok)
        throw new Error('Failed to fetch the thumbnail');
    // convert the response to a JSON object
    const jsonResponse = await response.json();
    // return the base64 thumbnail data and its MIME type
    return {
        base64Data: jsonResponse.data,
        mimeType: jsonResponse.mimeType
    };
}

/**
 * Initiates the processing of visible items. Processing is batched based on THUMBNAILS_RETRIEVAL_BATCH_SIZE. * 
 * @param {Array} visibleItemsList - List of visible items to be processed.
 */
async function processVisibleItems(visibleItemsList) {
    const visibleItems = [...visibleItemsList];
    abortController = new AbortController(); // create a new AbortController
    // process items in batches determined by THUMBNAILS_RETRIEVAL_BATCH_SIZE
    for (let i = 0; i < Math.min(THUMBNAILS_RETRIEVAL_BATCH_SIZE, visibleItems.length); i++) {
        const item = visibleItems.shift();
        processItem(item, visibleItems);
    }
}

/**
 * Returns the path to an SVG icon based on the file extension.
 * @param {string} filename - The full name of the file including its extension.
 * @returns {string} - The path to the appropriate SVG icon or a default one if not found.
 */
function getIconPathForFile(filename, type) {
    // extract file extension from filename
    const fileExtension = filename.split('.').pop().toLowerCase();
    // return the appropriate SVG image path or a default one if not found
    return FILE_ICONS[fileExtension] || (type === "file" ? "file.svg" : "directory.svg");
}

/**
 * Determines the appropriate SVG icon path based on environment type.
 * @param {Object} type - An environment type object.
 * @param {string} type.name - The name of the environment (e.g., "local", "gdrive").
 * @param {string} type.platformType - The platform type, if applicable (e.g., "windows", "unix").
 * @returns {string} The relative path to the appropriate SVG image.
*/
function getEnvironmentIconPath(type) {
    if (type.name !== "local") {
        return baseUrl + "/images/ui/" + type.name.toLowerCase() + ".svg";  // Example: "gdrive.svg"
    } else {
        return baseUrl + "/images/ui/" + type.platformType.toLowerCase() + ".svg"; // Example: "windows.svg" or "unix.svg"
    }
}

/**
 * Parses the current path
 * @param {any} needsContentRefresh - Whether the current tab needs updating or not
 */
function parsePath(needsContentRefresh) {
    const foundEnvironment = environmentTypes.find(environment => environment.id === activeEnvironmentId);
    const foundEnvironmentPlatformType = foundEnvironment.platformType;
    const separator = (foundEnvironmentPlatformType === "Unix") ? '/' : '\\';
    if (!addressBarInput.value.endsWith(separator))
        addressBarInput.value = addressBarInput.value + separator;
    const path = addressBarInput.value;
    if (IS_DEBUG)
        console.info(getCurrentTime() + " Preparing to parse path: " + path);
    if (needsContentRefresh) {
        // if there was a job for thumbnails retrieval, cancel it
        if (abortController) {
            abortController.abort();
            abortController = null;
        }
    }
    $.ajax({
        url: baseUrl + '/FileSystem/CheckPath?path=' + encodeURIComponent(path),
        type: 'GET',
        headers: {
            "X-Environment-Type": activeEnvironmentId.toString()
        },
        success: function (data) {
            if (data.success) {
                if (IS_DEBUG)
                    console.info(getCurrentTime() + " Checked path for: " + path);
                $.ajax({
                    url: baseUrl + '/FileSystem/ParsePath?path=' + encodeURIComponent(path),
                    type: 'GET',
                    headers: {
                        "X-Environment-Type": activeEnvironmentId.toString()
                    },
                    success: function (data) {
                        if (data.success) {
                            if (IS_DEBUG)
                                console.info(getCurrentTime() + " Parsed path for: " + path);
                            renderAddressBar(data.pathSegments);
                            addressBarInput.style.display = 'none'; // hide #addressBarInput
                            addressBar.style.display = 'block'; // show #addressBar
                            // find the last directory in the list of path segments
                            var lastDirectory = null;
                            for (var i = data.pathSegments.length - 1; i >= 0; i--) {
                                if (data.pathSegments[i].isDirectory) {
                                    lastDirectory = data.pathSegments[i];
                                    break;
                                }
                            }
                            if (needsContentRefresh)
                                updateCurrentTab(path, lastDirectory ? lastDirectory.name : (data.pathSegments.length > 0 ? data.pathSegments[data.pathSegments.length - 1].name + separator : "New Tab"));
                        }
                        else
                            console.error(data.errorMessage);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        if (jqXHR.status === 401) 
                            window.location.href = "/Account/Login";
                        else 
                            console.error('Failed to fetch files:', errorThrown);
                    }
                });
            }
            else
                console.error(data.errorMessage);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status === 401) // TODO: perhaps, update the last location of current tab, so that it returns to it after login?
                window.location.href = "/Account/Login"; 
            else 
                console.error('Failed to fetch files:', errorThrown);
        }
    });
}

/**
 * Render the address bar based on the provided path segments.
 * @param {Array<PathSegmentEntity>} pathSegments - An array of path segments.
 */
function renderAddressBar(pathSegments) {
    pathSegmentsContainer.removeEventListener('change', handlePathSegmentComboboxChange);  // remove previous listeners
    pathSegmentsContainer.addEventListener('change', handlePathSegmentComboboxChange);  // add new listener
    pathSegmentsContainer.innerHTML = ""; // clear existing segments
    if (pathSegments !== null) {
        pathSegments.forEach((segment, index) => {
            const li = document.createElement("li");
            // create the combobox
            const combobox = document.createElement("div");
            combobox.className = "navigator-combobox inline-block";
            // the shine effect 
            const shineEffect = document.createElement("div");
            shineEffect.className = "shine-effect";
            shineEffect.style.top = "1px";
            combobox.appendChild(shineEffect);
            const toggleCheckbox = document.createElement("input");
            toggleCheckbox.type = "checkbox";
            toggleCheckbox.className = "navigator-toggle-checkbox";
            toggleCheckbox.id = `segmentToggle_${index}`;
            combobox.appendChild(toggleCheckbox);
            const toggleLabel = document.createElement("label");
            toggleLabel.className = "navigator-toggle";
            toggleLabel.htmlFor = `segmentToggle_${index}`;
            const span = document.createElement("span");
            span.className = "navigator-selected-text";
            span.innerText = segment.name;
            toggleLabel.appendChild(span);
            // add the arrow element
            const arrowSpan = document.createElement("span");
            arrowSpan.className = "navigator-arrow";
            toggleLabel.appendChild(arrowSpan);
            combobox.appendChild(toggleLabel);
            const dropdown = document.createElement("div");
            dropdown.className = "navigator-dropdown";
            dropdown.id = `navigatorDropdown_${index}`;
            const foundEnvironment = environmentTypes.find(environment => environment.id === activeEnvironmentId);
            const foundEnvironmentPlatformType = foundEnvironment.platformType;
            const separator = (foundEnvironmentPlatformType === "Unix") ? '/' : '\\';
            // get the segments from start to the current one (on UNIX, start with path separator char!)
            const concatenatedPath = (foundEnvironmentPlatformType === "Unix" ? '/' : '') + pathSegments.slice(0, index + 1).map(seg => seg.name).join(separator);
            combobox.setAttribute('data-path', concatenatedPath + (!concatenatedPath.endsWith(separator) ? separator : ''));
            combobox.appendChild(dropdown);
            li.appendChild(combobox);
            pathSegmentsContainer.appendChild(li);
        });
    }
    else
        pathSegmentsContainer.querySelectorAll('li').forEach(li => li.remove()); // no address, clear path segments
}

/**
 * Handle the change event for the comboboxes inside the path-segments.
 * This function is triggered when any of the comboboxes (checkboxes) in the navigation bar is toggled.
 * @param {Event} event - The change event object. 
 */
function handlePathSegmentComboboxChange(event) {
    // Ensure the event was triggered by an element with the 'navigator-toggle-checkbox' class
    if (event.target.classList.contains('navigator-toggle-checkbox')) {
        const comboboxId = event.target.id;
        const dropdownId = comboboxId.replace('segmentToggle_', 'navigatorDropdown_');
        const dropdown = document.getElementById(dropdownId);
        reattachDropdown();
        if (event.target.checked) {
            console.log('Dropdown opened!');
            const comboboxElement = event.target.closest('.navigator-combobox'); // get the closest parent (or self) with the specified class
            const pathValue = comboboxElement.getAttribute('data-path');  // retrieve the data-path attribute value
            // clear any existing dropdown elements
            dropdown.innerHTML = "";
            // fetch directory data for the path.
            $.ajax({
                url: baseUrl + '/FileSystem/GetDirectories?path=' + encodeURIComponent(pathValue),
                type: 'GET',
                headers: {
                    "X-Environment-Type": activeEnvironmentId.toString()
                },
                success: function (directoriesData) {
                    if (directoriesData.success) {
                        // add combobox entry for current directory
                        const currentDirectoryDiv = document.createElement('div');
                        currentDirectoryDiv.className = "navigator-option";
                        currentDirectoryDiv.dataset.value = pathValue;
                        currentDirectoryDiv.textContent = ".";
                        currentDirectoryDiv.addEventListener('click', function (event) {
                            addressBarInput.value = pathValue;
                            parsePath(true);
                            console.log('Current directory option clicked:', event.target.textContent);
                        });
                        dropdown.appendChild(currentDirectoryDiv);
                        // add combobox entry for parent directory
                        const parentDirectoryDiv = document.createElement('div');
                        parentDirectoryDiv.className = "navigator-option";
                        parentDirectoryDiv.dataset.value = pathValue;
                        parentDirectoryDiv.textContent = "..";
                        parentDirectoryDiv.addEventListener('click', function (event) {
                            addressBarInput.value = pathValue;
                            goUpOneLevel();
                            console.log('Current directory option clicked:', event.target.textContent);
                        });
                        dropdown.appendChild(parentDirectoryDiv);
                        // add options for the rest of the directories
                        directoriesData.directories.forEach(option => {
                            const directoryDiv = document.createElement('div');
                            directoryDiv.className = "navigator-option";
                            directoryDiv.dataset.value = pathValue + option.name;
                            directoryDiv.textContent = option.name;
                            directoryDiv.addEventListener('click', function (event) {
                                const foundEnvironment = environmentTypes.find(environment => environment.id === activeEnvironmentId);
                                const foundEnvironmentPlatformType = foundEnvironment.platformType;
                                const separator = (foundEnvironmentPlatformType === "Unix") ? '/' : '\\';
                                addressBarInput.value = pathValue + (!pathValue.endsWith(separator) ? separator : '') + option.name + separator;
                                parsePath(true);
                                console.log('Current directory option clicked:', addressBarInput.value);
                            });
                            dropdown.appendChild(directoryDiv);
                        });
                    }
                },
                error: function (error) {
                    console.error('Failed to fetch directories:', error);
                }
            });
            // when drop down opens, need to show address bar overflow, otherwise clipping of drop down occurs
            // store the id of the parent combobox of the dropdown - the dropdown will be reparented 
            // because of manadatory "overflow hidden" of scrollable area and clipping isues
            dropdown.setAttribute('data-parent', event.target.id);
            dropdown.setAttribute('data-detached', true);
            const position = event.target.closest('.navigator-combobox').getBoundingClientRect();
            // Detach dropdown and reposition
            document.body.appendChild(dropdown);
            dropdown.style.position = 'absolute';
            dropdown.style.top = position.bottom + 'px';
            dropdown.style.left = position.left + 'px';
            dropdown.style.display = 'block';
            // handle addressBar scroll to adjust dropdown position
            addressBar.addEventListener('scroll', adjustDropdownPosition);
            console.log("added event");
        } else {
            console.log('Dropdown closed!');
        }
    }
}

/**
 * Adjusts the position of a detached drop down, such that it is always just under its original 
 * path segment combobox, even when this parent is moved through scrolling
 */
function adjustDropdownPosition() {
    const dropdown = document.querySelector('.navigator-dropdown[data-detached="true"]');
    if (dropdown) {
        const comboboxElement = document.getElementById(dropdown.getAttribute('data-parent')); // Use this to get the related combobox
        const position = comboboxElement.parentNode.getBoundingClientRect();
        dropdown.style.top = position.bottom + 'px';
        dropdown.style.left = position.left + 'px';
    }
}

/**
 * Re-attaches the detached visible drop down to the path segment or platform combobox it originally belonged to
 */
function reattachDropdown() {
    // when the combobox closes, re-attach the detached dropdown
    const dropdown = document.querySelector('.navigator-dropdown[data-detached="true"], .enlightenment-dropdown[data-detached="true"]');
    if (dropdown) {
        // reset its absolute positioning and stuff
        dropdown.style.position = '';
        dropdown.style.top = '';
        dropdown.style.left = '';
        dropdown.style.display = '';
        // re-attach it to original parent
        const dataParent = dropdown.getAttribute('data-parent');
        dropdown.removeAttribute('data-detached');
        const originalParent = document.getElementById(dataParent).parentNode;
        if (originalParent)
            originalParent.appendChild(dropdown);
        // remove scroll event listener from the addressBar
        addressBar.removeEventListener('scroll', adjustDropdownPosition);
    }
}

/**
 * Handle the change event for the comboboxes inside the path-segments.
 * This function is triggered when any of the comboboxes (checkboxes) in the navigation bar is toggled.
 * @param {Event} event - The change event object. 
 */
function handleEnvironemtComboboxChange(event) {
    reattachDropdown();
    if (event.target.checked) {
        environmentDropdown.setAttribute('data-parent', event.target.id);
        environmentDropdown.setAttribute('data-detached', true);
        const position = event.target.closest('.enlightenment-combobox').getBoundingClientRect();
        // detach dropdown and reposition
        document.body.appendChild(environmentDropdown);
        environmentDropdown.style.position = 'absolute';
        environmentDropdown.style.top = position.bottom + 'px';
        environmentDropdown.style.left = position.left + 'px';
        environmentDropdown.style.display = 'block';
        environmentDropdown.style.width = '54px';
    } 
}

/**
 * Adds a new user page to storage
 * @param {any} guid - The unique id of the page to add
 * @param {any} title - The title of the page to add
 * @param {any} path - The path of the page to add
 * @param {any} platformId - The id of the platform of the page to add
 */
function addNewPageToStorage(guid, title, path, platformId) {
    const page = {
        uuid: guid,
        title: title,
        path: path,
        platformId: platformId
    };
    $.ajax({
        url: baseUrl + '/FileSystem/AddPage',
        type: 'POST',
        data: JSON.stringify(page),
        contentType: "application/json; charset=utf-8",
        headers: {
            "X-Environment-Type": activeEnvironmentId.toString()
        },   
        error: function (jqXHR, textStatus, errorThrown) {
            swal("STOP!", textStatus + " " + errorThrown, "error", {
                button: {
                    text: "OK",
                    className: "confirm-button",
                }
            });
        }
    });
}

/**
 * Updates a page in the storage
 * @param {any} guid - The unique id of the page to update
 * @param {any} path - The path of the page to update
 * @param {any} environmentId - The id of the platform of the page to update
 * @param {any} title - The title of the page to update
 */
function updatePageInStorage(guid, path, environmentId, title) {
    const page = {
        uuid: guid,
        title: title,
        path: path,
        platformId: environmentId
    };
    $.ajax({
        url: baseUrl + '/FileSystem/UpdatePage',
        type: 'POST',
        data: JSON.stringify(page),
        contentType: "application/json; charset=utf-8",
        headers: {
            "X-Environment-Type": activeEnvironmentId.toString()
        },
        error: function (jqXHR, textStatus, errorThrown) {
            swal("STOP!", textStatus + " " + errorThrown, "error", {
                button: {
                    text: "OK",
                    className: "confirm-button",
                }
            });
        }
    });
}

/**
 * Deletes a page from the storage
 * @param {any} guid - The unique id of the page to delete
 */
function removePageFromStorage(guid) {
    $.ajax({
        url: baseUrl + '/FileSystem/RemovePage',
        type: 'POST',
        data: JSON.stringify(guid),
        contentType: 'application/json; charset=utf-8',
        headers: {
            "X-Environment-Type": activeEnvironmentId.toString()
        },
        error: function (jqXHR, textStatus, errorThrown) {
            swal("STOP!", textStatus + " " + errorThrown, "error", {
                button: {
                    text: "OK",
                    className: "confirm-button",
                }
            });
        }
    });
}

/**
 * Gets the permissions of the currently logged in user
 */
function getUserPermissions() {
    if (userId !== "") { // userId is set in _Layout.cshtml
        $.ajax({
            url: "/Permissions/GetPermissionsByUserId/" + userId,
            method: "GET",
            success: function (userPermissionsData) {
                if (userPermissionsData.success) {
                    setUserPermissions(userPermissionsData.userPermissions.map(p => p.permissionName))
                    updatePrimaryMenuItemsVisibility(userPermissionsData.userPermissions);
                    updateSecondaryMenuItemsVisibility(userPermissionsData.userPermissions);
                    $.ajax({
                        url: "/Permissions/GetRolesByUserId/" + userId,
                        method: "GET",
                        success: function (userRolesData) {
                            if (userRolesData.success) {
                                var userRoles = userRolesData.userRoles;
                                // iterate roles and get their permissions
                                for (var i = 0; i < userRoles.length; i++) {
                                    $.ajax({
                                        url: "/Permissions/GetPermissionsByRoleId/" + userRoles[i].id,
                                        method: "GET",
                                        success: function (rolePermissionsData) {
                                            if (rolePermissionsData.success) {
                                                setUserPermissions(rolePermissionsData.rolePermissions.map(p => p.permissionName))
                                                updatePrimaryMenuItemsVisibility(rolePermissionsData.rolePermissions);
                                                updateSecondaryMenuItemsVisibility(rolePermissionsData.rolePermissions);
                                            } else {
                                                if (rolePermissionsData.errorMessage)
                                                    swal("STOP!", rolePermissionsData.errorMessage, "error", {
                                                        button: {
                                                            text: "OK",
                                                            className: "confirm-button",
                                                        }
                                                    }); 
                                            }
                                        }
                                    });
                                }
                            } else {
                                if (userRolesData.errorMessage)
                                    swal("STOP!", userRolesData.errorMessage, "error", {
                                        button: {
                                            text: "OK",
                                            className: "confirm-button",
                                        }
                                    });
                            }
                        }
                    });
                } else {
                    if (userPermissionsData.errorMessage)
                        swal("STOP!", userPermissionsData.errorMessage, "error", {
                            button: {
                                text: "OK",
                                className: "confirm-button",
                            }
                        });
                }
            }
        });
    }
}

// store the user permissions for the remaining of this session
// note: chill, this is for UI visual manipulation purposes ONLY, the permissions are checked server side anyway!
function setUserPermissions(permissions) {
    window.Permissions = [...new Set([...window.Permissions, ...permissions])];
}

/**
 * Determines if the given element has vertical and/or horizontal scrollbars.
 * @param {HTMLElement} element - The element to check for scrollbars.
 * @returns {Object} An object indicating the presence of vertical and horizontal scrollbars.
 * @property {boolean} vertical - True if the element has a vertical scrollbar, otherwise false.
 * @property {boolean} horizontal - True if the element has a horizontal scrollbar, otherwise false.
 */
function hasScrollbars(element) {
    const tolerance = 2;
    return {
        vertical: element.scrollHeight - element.clientHeight > tolerance,
        horizontal: element.scrollWidth - element.clientWidth > tolerance
    };
}

/**
 * Get the height of the browser's scrollbar.
 * @returns {number} Height of the scrollbar in pixels.
 */
function getScrollbarHeight() {
    // create an outer div and set its height and overflowY properties
    const outer = document.createElement('div');
    outer.style.visibility = 'hidden';
    outer.style.height = '100px';
    outer.style.overflowY = 'scroll';
    document.body.appendChild(outer);
    const heightNoScroll = outer.offsetHeight;
    const heightWithScroll = outer.clientHeight;
    // cleanup after measuring
    outer.parentNode.removeChild(outer);
    return heightNoScroll - heightWithScroll;
}

/**
 * Get the width of the browser's scrollbar.
 * @returns {number} Width of the scrollbar in pixels.
 */
function getScrollbarWidth() {
    // create an outer div and set its width
    const outer = document.createElement('div');
    outer.style.visibility = 'hidden';
    outer.style.width = '100px';
    document.body.appendChild(outer);
    const widthNoScroll = outer.offsetWidth;
    // apply overflow and append an inner div
    outer.style.overflow = 'scroll';
    const inner = document.createElement('div');
    inner.style.width = '100%';
    outer.appendChild(inner);
    const widthWithScroll = inner.offsetWidth;
    // cleanup and calculate the difference
    outer.parentNode.removeChild(outer);
    return widthNoScroll - widthWithScroll;
}

/**
 * Generates a UUID conforming to RFC4122 version 4.
 * @returns {string} A version 4 UUID string.
 */
function generateUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0; // generate a random number between 0-15
        // depending on whether character is 'x' or 'y', compute the value for that position in UUID
        var v = c == 'x' ? r : (r & 0x3 | 0x8);
        // r & 0x3 ensures the result is between 0-3
        // | 0x8 sets the high bit, ensuring result is one of 8, 9, A, or B, as per RFC4122
        return v.toString(16); // convert the number to a hexadecimal string
    });
}

$(document).ready(function () {

    // ==================================
    // Combobox
    // ==================================

    /**
    * Event handler for when an option in the combobox is clicked.
    * This updates the displayed value of the combobox to the clicked option
    * and closes the dropdown.
    */
    $(document).on("click", ".enlightenment-option:not(#environmentsDropdown .enlightenment-option)", function () {
        var value = $(this).data("value");
        var text = $(this).text();
        var combobox = $(this).closest(".enlightenment-combobox");
        combobox.find(".enlightenment-selected-text").text(text);
        combobox.find(".enlightenment-toggle-checkbox").prop("checked", false);
    });

    /**
     * Event handler for environment combobox drop down items click.
     */
    $(document).on("click", "#environmentsDropdown .enlightenment-option", function () {
        reattachDropdown();
        const clickedImage = $(this).find('img');
        const environment = environmentTypes.find(item => item.id === clickedImage.data("environemt-id"));

        var combobox = $(this).closest(".enlightenment-combobox");
        combobox.find("label.enlightenment-toggle img").attr('src', clickedImage.attr('src'));
        combobox.find(".enlightenment-toggle-checkbox").prop("checked", false);
        const titleValue = (environment.title === "Local File System") ? `${environment.title} (${environment.platformType})` : environment.title;

        combobox.attr('title', titleValue); 
        const imgElement = combobox.find("label.enlightenment-toggle img");
        imgElement.attr('title', titleValue);
        imgElement.attr('alt', titleValue);
        // TODO: store the selected platform id and use it for creating new explorer tabs,  navigating current ones to default platform path, etc
        const explorer = getActiveExplorer();
        if (explorer) 
            explorer.setAttribute('data-environment', environment.id);
        addressBarInput.value = environment.initialPath;
        activeEnvironmentId = environment.id;
        parsePath(true);
    });

    /**
     * Event handler for navigation combobox drop down item clicks.
     */
    $(document).on("click", ".navigator-option", function () {
        // the address bar drop downs may be destroyed when an option is clicked - they are regenerated each time anyway
        addressBar.removeEventListener('scroll', adjustDropdownPosition);
        $(this).parent().remove();
    });

    /**
     * Detects changes on any combobox checkbox. If a combobox is opened, this
     * will ensure all other comboboxes are closed.
     */
    $(document).on('change', '.enlightenment-toggle-checkbox, .navigator-toggle-checkbox', function () {
        if ($(this).prop('checked')) 
            closeAllComboboxesExcept($(this)); 
    });

    /**
     * Global click event for the entire document. Closes all combobox dropdowns
     * if the clicked target is outside any combobox.
     */
    $(document).click(function (event) {
        if (!$(event.target).closest('.enlightenment-combobox').length) {
            const checkbox = $('.enlightenment-toggle-checkbox:checked').prop('checked', false);
            checkbox.prop('checked', false);
        }
        if (!$(event.target).closest('.navigator-combobox').length && addressBar) {
            const checkbox = $('.navigator-toggle-checkbox:checked').prop('checked', false);
            checkbox.prop('checked', false);
            // when drop down closes, hide address bar overflow
            addressBar.style.overflowX = 'auto';
            addressBar.style.overflowY = 'hidden';
            reattachDropdown();
        }
        accountDropdown.classList.add('hidden');
    });

    /**
     * Prevents the global document click event from propagating when 
     * clicking on a combobox. This ensures the dropdown doesn't close 
     * unintentionally.
     */
    $(".enlightenment-combobox, .navigator-combobox").click(function (event) {
        event.stopPropagation();
    });

    /**
     * Closes all comboboxes except for the one passed as an argument.
     * @param {jQuery} exceptionCheckbox - The checkbox of the combobox that should remain open.
     */
    function closeAllComboboxesExcept(exceptionCheckbox) {
        $('.enlightenment-toggle-checkbox:checked, .navigator-toggle-checkbox:checked').each(function () {
            if (!exceptionCheckbox || $(this).attr('id') !== exceptionCheckbox.attr('id')) {
                $(this).prop('checked', false);
            }
        });
    }
    
    if (environmentDropdown)
        // add the available environments to the environments combobox in the navigator
        environmentTypes.forEach((type, index, array) => {
            const optionDiv = document.createElement('div');
            optionDiv.className = "enlightenment-option";
            if (index === array.length - 1)
                optionDiv.classList.add("last-option");
            // create an image element and set its source to the appropriate SVG
            const imgElement = document.createElement('img');
            imgElement.src = getEnvironmentIconPath(type);
            imgElement.alt = type.title;
            imgElement.title = type.title;
            imgElement.style.width = '42px';
            imgElement.setAttribute('data-environemt-id', type.id);
            optionDiv.appendChild(imgElement);
            optionDiv.style.padding = '0px';
            environmentDropdown.appendChild(optionDiv);
        });
});