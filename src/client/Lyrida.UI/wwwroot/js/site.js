// test placeholders
const environmentTypes = [
    // { title: "Home", path: "/app" },
    { id: 1, title: "Local File System", "name": "local", platformType: "unix", initialPath: "/app/" },
    { id: 2, title: "Local File System", "name": "local", platformType: "windows", initialPath: "C:\\Users\\Andromeda\\Desktop" },
    { id: 3, title: "File Transfer Protocol", "name": "ftp", platformType: "unix", initialPath: "ftp://user:pasword@hostname:21/home/user" },
    { id: 4, title: "Google Drive", "name": "gdrive", platformType: "unix", initialPath: "/test/address/" },
];

// ==================================
// Constants
// ==================================
// TODO: remove in production!
const PATH_CHAR_SEPARATOR = "/";
// Development mode
const IS_DEBUG = true;
// Customizable timeout duration for scroll actions.
const SCROLL_TIMEOUT_DURATION = 1000;
// Determines the batch size for thumbnails retrieval.
const THUMBNAILS_RETRIEVAL_BATCH_SIZE = 20;
// Current icon pack theme
const CURRENT_ICON_THEME = "Lyra";
// whether to show thumbnails or not
const SHOW_THUMBNAILS = true;
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
    "svg": "application-msoutlook.svg",
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

const inputField = document.getElementById('addressBarInput');
const addressBar = document.getElementById('address-bar');
const pathSegmentsContainer = document.getElementById("pathSegments");

/**
 * Retrieves the explorer container element.
 * @returns {HTMLElement} - The explorer container element.
 */
function getExplorerContainer() {
    return document.getElementById(`explorerContainer${activeTabId}`);
}

/**
 * Retrieves the active details header element.
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

let scrollTimeout; // timeout to delay thumbnail loading after scrolling
let resizeTimeout; // timeout to delay thumbnail loading after resizing
let hasScrolledAfterModeChange = false; // flag to track if scrolling occurred after a mode change
let abortController; // allows the cancellation of thumbnail retrieval jobs
let activeTabId; // tracks the currently active tab's ID

// ==================================
// Event Handlers
// ==================================

/**
 * Binds event listeners to the active explorer and its container.
 */
function bindEventsToActiveExplorer() {
    const explorer = getActiveExplorer();
    const explorerContainer = getExplorerContainer();
    // remove any previously bound event listeners
    unbindEventsFromExplorer(explorer, explorerContainer);
    explorer.addEventListener('scroll', handleScrollEvent);
    explorerContainer.addEventListener('scroll', handleScrollEvent);
    explorer.addEventListener('wheel', scrollHorizontally);
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
    explorer.removeEventListener('wheel', scrollHorizontally);
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
        if (SHOW_THUMBNAILS)
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
        if (SHOW_THUMBNAILS)
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

// ==================================
// Event Listeners Initializations
// ==================================

// adjust thumbnails upon window resizing
window.addEventListener('resize', handleResizeEvent);

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
        if (!hasScrolledAfterModeChange && SHOW_THUMBNAILS) getVisibleItems();
    }, 50); // wait 50ms to ensure all other events have processed
}

/**
 * Sets the explorer to 'Details' view mode, displaying extended item details.
 */
function setDetailsViewMode() {
    const explorer = getActiveExplorer();
    const explorerContainer = getExplorerContainer();
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

        explorer.removeEventListener('wheel', scrollHorizontally);

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
    const explorerContainer = getExplorerContainer();
    const detailsHeader = getActiveDetailsHeader();

    if (explorer) {
        // update DOM elements to reflect 'List' view mode
        explorer.classList.remove('Details');
        explorer.classList.add('List');
        explorer.style.flexDirection = "column";

        explorerContainer.classList.remove('scrollVertical');
        explorerContainer.classList.add('scrollHorizontal');

        detailsHeader.style.height = "0px";

        explorer.addEventListener('wheel', scrollHorizontally);

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
    const explorerContainer = getExplorerContainer();
    const detailsHeader = getActiveDetailsHeader();

    if (explorer) {
        // update DOM elements to reflect 'Icons' view mode
        explorer.classList.remove('List');
        explorer.classList.add('Details');
        explorer.style.flexDirection = "row";

        explorerContainer.classList.remove('scrollHorizontal');
        explorerContainer.classList.add('scrollVertical');

        detailsHeader.style.height = "0px";

        explorer.removeEventListener('wheel', scrollHorizontally);

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
 * @param {Function} callback - A function to be executed after successful data retrieval.
 */
function fetchDataForPath(path, callback) {
    // fetch directory data for the path.
    $.ajax({
        url: baseUrl + '/FileSystem/GetDirectories?path=' + encodeURIComponent(path),
        type: 'GET',
        success: function (directoriesData) {
            if (directoriesData.success) {
                // fetch file data, upon successful directory fetch.
                $.ajax({
                    url: baseUrl + '/FileSystem/GetFiles?path=' + encodeURIComponent(path),
                    type: 'GET',
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
}

/**
 * Adds a new tab with its title and path, optionally with a specified ID.
 * Fetches directory and file data for the tab and renders its content.
 * @param {string} title - The title of the tab.
 * @param {string} path - The file path associated with the tab.
 * @param {number} [id] - The optional ID for the tab. If not provided, it increments a global ID.
 */
function addNewTab(title, path, id) {
    // determine the tab ID
    const currentTabId = (typeof id !== 'undefined') ? id : nextTabId++;
    // construct the new page object
    const newPage = { title: title, path: path, id: currentTabId };
    initialPages.push(newPage);
    // render the new tab's header
    renderTab(newPage);
    // set the current path as the title of the tab header
    document.getElementById(`tabHeader${currentTabId}`).setAttribute("title", path);
    // fetch directory data for the new tab
    fetchDataForPath(path, function (data) {
        // Render the tab's content and bind events.
        renderTabContent(currentTabId, data);
        bindEventsToActiveExplorer();
        switchViewMode(setListViewMode);
    });
}

/**
 * Updates the content of the currently active tab with the data from the provided path.
 * @param {string} path - The file system path to fetch data from.
 */
function updateCurrentTab(path, title) {
    const explorer = getActiveExplorer();
    if (explorer) {
        // update the tab header title
        let tabHeader = document.getElementById(`tabHeader${activeTabId}`);
        explorer.setAttribute("data-path", path);
        tabHeader.getElementsByTagName('a')[0].textContent = title;
        fetchDataForPath(path, function (data) {
            // update the current tab's content with the fetched data
            updateTabContent(activeTabId, data);
            // rebind necessary event handlers to the updated content
            bindEventsToActiveExplorer();
            switchViewMode(setListViewMode);
        });
    }
    else
        addNewTab(title, path);
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
    // update address bar input with current explorer path
    const path = document.getElementById(`explorer${tabId}`).getAttribute('data-path');
    $('#addressBarInput').val(path);
    parsePath(false);
    // update the active tab id
    activeTabId = tabId;
    if (SHOW_THUMBNAILS)
        getVisibleItems(); // get currently visible items in the tab
}

/**
 * Renders and initializes a new tab in the UI based on provided page data.
 * @param {Object} page - The page data for the new tab, containing at least 'id' and 'title'.
 */
function renderTab(page) {
    // create tab header
    let tabHeader = document.createElement("li");
    tabHeader.id = `tabHeader${page.id}`;
    // create tab link
    let tabLink = document.createElement("a");
    tabLink.href = "#";
    tabLink.innerText = page.title;
    // create close button for the tab
    let closeButton = document.createElement("a");
    closeButton.href = "#";
    closeButton.className = "closeIcon";
    closeButton.title = "Close";
    closeButton.addEventListener('click', (e) => {
        e.preventDefault();
        e.stopPropagation(); // prevent tab selection post close
        // check if current tab is active
        const isActive = $(tabHeader).hasClass('active');
        // determine next and previous tabs
        const nextTab = $(tabHeader).next('li');
        const prevTab = $(tabHeader).prev('li');
        // remove current tab header and content
        tabHeader.remove();
        $(`#tabPage${page.id}`).remove();
        // remove tab data from initialPages array
        const index = initialPages.findIndex(p => p.id === page.id);
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
    });
    // append tab link and close button to the tab header
    tabHeader.appendChild(tabLink);
    tabHeader.appendChild(closeButton);
    // switch to tab when its header is clicked
    tabHeader.addEventListener('click', (e) => {
        e.preventDefault();
        e.stopPropagation(); // prevent tab selection post close
        switchTab(page.id)
    });
    // create tab content container
    let tabPage = document.createElement("div");
    tabPage.id = `tabPage${page.id}`;
    // insert the tab header and content container into DOM
    $("#dynTab_tabHeaders").prepend(tabHeader);
    $("#dynTab_tabPages").append(tabPage);
}

/**
 * Renders the content for a given tab using provided data.
 * @param {string} tabId - The unique identifier for the tab.
 * @param {Object} data - Data containing directories and files to be rendered.
 */
function renderTabContent(tabId, data) {
    // DOM Elements
    const drivesContainer = createContainer(`drivesContainer${tabId}`);
    const explorerContainer = createContainer(`explorerContainer${tabId}`, "scrollHorizontal");
    const detailsHeader = createDetailsHeader(tabId);
    const explorer = createExplorer(tabId, data.path);
    const previewContainer = createContainer(`previewContainer${tabId}`);
    // populate explorer with directories and files
    populateExplorerWithData(explorer, data);
    // appending elements to the DOM
    explorerContainer.appendChild(detailsHeader);
    explorerContainer.appendChild(explorer);
    const tabPage = document.querySelector(`#tabPage${tabId}`);
    tabPage.appendChild(drivesContainer);
    tabPage.appendChild(explorerContainer);
    tabPage.appendChild(previewContainer);
    // switch to the newly rendered tab
    switchTab(tabId);
}

function updateTabContent(tabId, data) {    
    const explorer = getActiveExplorer();
    while (explorer.firstChild)
        explorer.removeChild(explorer.firstChild); // clear the current content.
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
 * @returns {HTMLElement} - Returns the created explorer container.
 */
function createExplorer(tabId, path) {
    const explorer = createContainer(`explorer${tabId}`, "List");
    explorer.setAttribute("data-path", path);
    explorer.style.flexDirection = "column";
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
    entityDiv.className = "e list-icons";
    entityDiv.dataset.path = entity.path;
    entityDiv.dataset.type = type;        
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
    fileImg.src = baseUrl + "/images/icons/" + CURRENT_ICON_THEME  + "/" + getIconPathForFile(path, type);
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
                item.img.src = baseUrl + "/images/icons/" + CURRENT_ICON_THEME + "/d.png";
            else if (itemType === 'file')
                item.img.src = baseUrl + "/images/icons/" + CURRENT_ICON_THEME + "/f.png";
            console.error('Error processing item:', error);
        }
    }
}

/**
 * Makes an API call to retrieve a thumbnail for the given item.
 * @param {string} item - The item for which the thumbnail needs to be fetched.
 * @returns {Promise<object>} - Returns a Promise resolving to an object containing base64Data and mimeType.
 * @throws {Error} - Throws an error if the API call fails.
 */
async function getThumbnailApiCall(item) {
    // URL encode the item to ensure it's safely transmitted in the URL
    const encodedItem = encodeURIComponent(item);
    // fetch the thumbnail for the given item from the server
    const response = await fetch(baseUrl + `/FileSystem/GetThumbnail/${encodedItem}`, {
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
    return FILE_ICONS[fileExtension] || (type === "file" ? "f.png" : "d.png");
}

function parsePath(needsContentRefresh) {
    const path = $('#addressBarInput').val();
    $.ajax({
        url: baseUrl + '/FileSystem/CheckPath?path=' + encodeURIComponent(path),
        type: 'GET',
        success: function (data) {
            if (data.success) {
                $.ajax({
                    url: baseUrl + '/FileSystem/ParsePath?path=' + encodeURIComponent(path),
                    type: 'GET',
                    success: function (data) {
                        if (data.success) {
                            renderAddressBar(data.pathSegments);
                            inputField.style.display = 'none'; // Hide #addressBarInput
                            addressBar.style.display = 'block'; // Show #address-bar
                            // find the last directory in the list of path segments
                            var lastDirectory = null;
                            for (var i = data.pathSegments.length - 1; i >= 0; i--) {
                                if (data.pathSegments[i].isDirectory) {
                                    lastDirectory = data.pathSegments[i];
                                    break;
                                }
                            }
                            if (needsContentRefresh)
                                updateCurrentTab(path, lastDirectory ? lastDirectory.name : "New Tab");
                        }
                        else
                            console.error(data.error);
                    },
                    error: function (error) {
                        console.error('Failed to fetch files:', error);
                    }
                });
            }
            else
                console.error(data.error);
        },
        error: function (error) {
            console.error('Failed to fetch files:', error);
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

    pathSegments.forEach((segment, index) => {
        const li = document.createElement("li");

        // create the combobox
        const combobox = document.createElement("div");
        combobox.className = "navigator-combobox w-250px inline-block";
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

        
        const concatenatedPath = pathSegments.slice(0, index + 1).map(seg => seg.name).join(PATH_CHAR_SEPARATOR); // get the segments from start to the current one. TODO: Change to the tab's platform path separator char!
        combobox.setAttribute("data-path", concatenatedPath);
       
        ////dropdown.appendChild(option);
        combobox.appendChild(dropdown);

        li.appendChild(combobox);
        pathSegmentsContainer.appendChild(li);
    }); 
}

/**
 * Handle the change event for the comboboxes inside the path-segments.
 * This function is triggered when any of the comboboxes (checkboxes) in the navigation bar is toggled.
 * @param {Event} e - The change event object. 
 */
function handlePathSegmentComboboxChange(e) {
    // Ensure the event was triggered by an element with the 
    // 'navigator-toggle-checkbox' class
    if (e.target.classList.contains('navigator-toggle-checkbox')) {
        if (e.target.checked) {
            console.log('Dropdown opened!');
            const comboboxElement = e.target.closest('.navigator-combobox'); // get the closest parent (or self) with the specified class
            const pathValue = comboboxElement.getAttribute("data-path");  // retrieve the data-path attribute value

            const dropdown = e.target.closest('.navigator-combobox').querySelector('.navigator-dropdown');
            // clear any existing dropdown elements
            dropdown.innerHTML = "";


            // fetch directory data for the path.
            $.ajax({
                url: baseUrl + '/FileSystem/GetDirectories?path=' + encodeURIComponent(pathValue),
                type: 'GET',
                success: function (directoriesData) {
                    if (directoriesData.success) {      
                        // add combobox entry for current directory
                        const currentDirectoryDiv = document.createElement('div');
                        currentDirectoryDiv.className = "navigator-option";
                        currentDirectoryDiv.dataset.value = pathValue;
                        currentDirectoryDiv.textContent = ".";
                        currentDirectoryDiv.addEventListener('click', function (event) {
                            $('#addressBarInput').val(pathValue); 
                            parsePath(true);
                            console.log('Current directory option clicked:', event.target.textContent);
                        });

                        dropdown.appendChild(currentDirectoryDiv);
                        // TODO: add option for "..", after implementing "navigate up"

                        directoriesData.directories.forEach(option => {
                            const directoryDiv = document.createElement('div');
                            directoryDiv.className = "navigator-option";
                            directoryDiv.dataset.value = pathValue + option.name;
                            directoryDiv.textContent = option.name;
                            directoryDiv.addEventListener('click', function (event) {
                                $('#addressBarInput').val(pathValue + PATH_CHAR_SEPARATOR + option.name); // TODO: replace hardcoded path separator
                                parsePath(true);
                                console.log('Current directory option clicked:', pathValue + PATH_CHAR_SEPARATOR + option.name); 
                            });
                            dropdown.appendChild(directoryDiv);
                        });
                    }
                },
                error: function (error) {
                    console.error('Failed to fetch directories:', error);
                }
            });


            //const options = [
            //    { id: "test1", name: "Test Option w1" },
            //    { id: "test2", name: "Test Option e2" }, 
            //    { id: "test3", name: "Test Option 3t" }
            //];

            //options.forEach(option => {
            //    const optionDiv = document.createElement('div');
            //    optionDiv.className = "navigator-option";
            //    optionDiv.dataset.value = option.id;
            //    optionDiv.textContent = option.name;

            //    dropdown.appendChild(optionDiv);
            //});

        } else {
            console.log('Dropdown closed!');
        }
    }
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
    $(document).on("click", ".enlightenment-option, .navigator-option", function () {
        var value = $(this).data("value");
        var text = $(this).text();
        var combobox = $(this).closest(".enlightenment-combobox, .navigator-combobox");
        combobox.find(".enlightenment-selected-text, .navigator-selected-text").text(text);
        combobox.find(".enlightenment-toggle-checkbox, .navigator-toggle-checkbox").prop("checked", false);
    });

    /**
     * Detects changes on any combobox checkbox. If a combobox is opened, this
     * will ensure all other comboboxes are closed.
     */
    $(document).on('change', '.enlightenment-toggle-checkbox, .navigator-toggle-checkbox', function () {
        if ($(this).prop('checked')) {
            closeAllComboboxesExcept($(this));
        }
    });

    /**
     * Global click event for the entire document. Closes all combobox dropdowns
     * if the clicked target is outside any combobox.
     */
    $(document).click(function (event) {
        if (!$(event.target).closest('.enlightenment-combobox, .navigator-combobox').length) {
            $('.enlightenment-toggle-checkbox:checked, .navigator-toggle-checkbox:checked').prop('checked', false);
        }
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

    document.querySelectorAll('.navigator-toggle-checkbox').forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            if (this.checked) {
                console.log('Dropdown opened1!');
            } else {
                console.log('Dropdown closed2!');
            }
        });
    });
});

