// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded',()=>{

    const themeToggleBtn = document.getElementById('theme-toggle');
    const body = document.body;

    if(!themeToggleBtn){
        return;
    }
    const themeIcon = themeToggleBtn.querySelector('i');

    function applyTheme(theme) {
        if(theme === 'dark') {
            body.classList.add('dark-mode');
            themeIcon.classList.remove('fa-moon');
            themeIcon.classList.add('fa-sun');
        }
        else {
            body.classList.add('light-mode');
            themeIcon.classList.remove('fa-sun');
            themeIcon.classList.add('fa-moon');
        }

        const saveTheme = localStorage.getItem('theme' || 'light');
        applyTheme(saveTheme);

        themeToggleBtn.addEventListener('click', ()=> {
            const currentTheme = body.classList.contains('dark-mode') ? 'dark' : 'light';
            const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

            applyTheme(newTheme);
            localStorage.setItem('theme', newTheme);
        });
    }
})