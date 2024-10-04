(() => {
    // 維持寫死website
    const UTM_MEDIUM = 'website';
    // 取得當前頁面之html title。(前提是各頁title可識別差異)
    const UTM_CAMPAIGN = (() => {
        let currentPath = encodeURI(window.location.pathname).split('/');
        let name = currentPath.pop();
        if (!name) {
            name = '第一銀行基金理財網';
        }
        name = name.toLocaleLowerCase().replace(' ', '-');

        // default > 優先Html Title > Uri last part > 第一銀行基金理財網
        return document.title || name;
    })();

    // wms更換為最終子網域名稱
    const UTM_SOURCE = 'wealth';
    const BLOCKLIST = ['facebook.com/sharer/', 'social-plugins.line.me/lineit/share/', 'access.line.me/oauth2/'];
    const QUERYSELECTOR = 'a[href^="/"],a[href^="https://"],a[href^="http://"]';
    const PROP = 'href';
    /**
     * 增加連結QueryString
     * @param {Node} htmlNodes HtmlNodes
     */
    async function AddUTMCode(htmlNodes) {
        // 如果查無結果
        if (!htmlNodes || !htmlNodes.length) {
            return Promise.resolve();
        }
        htmlNodes.forEach(node => {
            try {
                let url = new URL(node.href);

                // 檢查是否為外站連結並需要添加 rel="noopener noreferrer"
                if (node.target === '_blank') {
                    let hostName = location.hostname;
                    if (url.hostname !== hostName && !/noopener|noreferrer/.test(node.rel)) {
                        node.rel = "noopener noreferrer";
                    }
                }

                let predict = BLOCKLIST.find(domain => {
                    let reg = new RegExp(domain, 'gi');
                    return reg.test(`${url.hostname}${url.pathname}`);
                });
                // 如果黑名單中
                if (predict) {
                    return;
                }

                let search = url.searchParams;
                let utmSourceKey = 'utm_source', utmMediumKey = 'utm_medium', utmCampaignKey = 'utm_campaign';

                search.delete(utmSourceKey);
                search.append(utmSourceKey, UTM_SOURCE);

                search.delete(utmMediumKey);
                search.append(utmMediumKey, UTM_MEDIUM);

                search.delete(utmCampaignKey);
                search.append(utmCampaignKey, UTM_CAMPAIGN);

                let setUrl;
                // 透過getAttribute查詢raw value判別內外部連結
                if (node.getAttribute(PROP).startsWith('/')) {
                    setUrl = `${url.pathname}${url.search}${url.hash}`;
                } else {
                    setUrl = url;
                }

                // 若setUrl非空值
                if (setUrl) {
                    node.setAttribute(PROP, setUrl);
                }
            } catch (e) {
                console.error(e);
            }
        })
        return Promise.resolve();
    }

    /**
     * 尋找需要修改的節點
     * @param {Node} node Node
     */
    async function FindNode(node) {
        let eles = node.querySelectorAll(QUERYSELECTOR);
        await AddUTMCode(eles);
        return Promise.resolve();
    }

    // 建立觀察者
    const observer = new MutationObserver((mutationList, observer) => {
        for (const mutation of mutationList) {
            if (mutation.type === "childList" && mutation.addedNodes && mutation.addedNodes.length) {
                mutation.addedNodes.forEach(node => {
                    if (node.nodeType === 1) {
                        // 如果新增的節點中，查詢到指定項目
                        FindNode(node);
                    }
                })
            }
        }
    });
    // 觀察子代
    observer.observe(document.body, { subtree: true, childList: true });

    // First View
    FindNode(document).catch(e => console.log(e));
})()