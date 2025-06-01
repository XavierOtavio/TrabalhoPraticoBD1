
/* BEGIN EXTERNAL SOURCE */

        function showSubMenu(id) {
            document.querySelectorAll('.submenu').forEach(el => el.style.display = 'none');
            var sub = document.getElementById('submenu_' + id);
            if (sub) sub.style.display = 'block';
        }

        function showTopicMenu(id) {
            var topic = document.getElementById('topicmenu_' + id);
            if (topic) topic.style.display = 'block';
        }
    
/* END EXTERNAL SOURCE */
