 function SelectTab(button) {
        debugger;
        const key = $(button).attr("key");
        const TabControl = $(button).closest('.TabControl');
        const tabcontent = $(TabControl).find('.tabcontent');
        const tabs = $(tabcontent).find('.tab');
        const tabHeader = $(TabControl).find('.tabHeader');
           
        for (let i = 0; i < tabHeader.length; i++) {
            tabHeader[i].classList.remove("selected");
        }
        button.classList.add("selected");

        for (let i = 0; i < tabs.length; i++) {
            tabs[i].classList.remove("selected");
            if ($(tabs[i]).attr("key") === key) {
                tabs[i].classList.add("selected");
            }
                        
        }
    }