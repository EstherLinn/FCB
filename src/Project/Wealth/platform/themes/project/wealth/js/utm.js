(() => {
    const CAMPAIGN_TYPE = 'url';
    // 維持寫死website
    const UTM_MEDIUM = 'website';
    // 取得當前頁面之html title。(前提是各頁title可識別差異)
    const UTM_CAMPAIGN = (() => {
        let currentPath = encodeURI(window.location.pathname).split('/');
        let name = currentPath.pop();

        if (CAMPAIGN_TYPE === 'url') {
            return name.toLocaleLowerCase().replace(' ', '-');
        }

        // default
        return document.title || name;
    })()
    // wms更換為最終子網域名稱
    const UTM_SOURCE = 'wms.firstbank.com.tw';
    const BLOCKLIST = ['facebook.com'];
    const QUERYSELECTOR = 'a[href^="/"],a[href^="https://"],a[href^="http://"]';

    /**
     * 增加連結QueryString
     * @param {Node} htmlNodes HtmlNodes
     */
    async function AddUTMCode(htmlNodes) {
        // 如果查無結果
        if (!htmlNodes || !htmlNodes.length) {
            return;
        }
        htmlNodes.forEach(node => {
            try {
                let url = new URL(window.encodeURI(node.href));

                let predict = BLOCKLIST.find(domain => {
                    let reg = new RegExp(domain, 'gi');
                    return reg.test(url.hostname);
                });
                // 如果黑名單中
                if (predict) {
                    return;
                }

                const search = url.searchParams;
                const utmSourceKey = 'utm_source', utmMediumKey = 'utm_medium', utmCampaignKey = 'utm_campaign';

                search.delete(utmSourceKey);
                search.append(utmSourceKey, UTM_SOURCE);

                search.delete(utmMediumKey);
                search.append(utmMediumKey, UTM_MEDIUM);

                search.delete(utmCampaignKey);
                search.append(utmCampaignKey, UTM_CAMPAIGN);

                let encodeLink = encodeURI(url.toString());
                node.href = encodeLink;

            } catch (e) {
                console.error(e);
            }
        })
    }

   /**
    * 尋找需要修改的節點
    * @param {Node} node Node
    */
    async function FindNode(node) {
        let eles = node.querySelectorAll(QUERYSELECTOR);
        await AddUTMCode(eles);
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
    document.addEventListener("DOMContentLoaded", function () {
        FindNode(document);
    });
})()