document.addEventListener('DOMContentLoaded', function () {
    
    console.log("DEBUG: DOM Siap. Script todolist dimulai.");

    const themeToggleBtn = document.getElementById('theme-toggle');
    const htmlElement = document.documentElement;
    
    console.log("DEBUG: Mencari tombol...", themeToggleBtn, " isi html",htmlElement);

    if (!themeToggleBtn) {
        console.error("DEBUG: Tombol #theme-toggle TIDAK DITEMUKAN. Script berhenti.");
        return;
    }

    const themeIcon = themeToggleBtn.querySelector('i');

    function applyTheme(theme) {
        htmlElement.setAttribute('data-bs-theme', theme);
        if (theme === 'dark') {
            themeIcon.classList.remove('fa-moon');
            themeIcon.classList.add('fa-sun');
        } else {
            themeIcon.classList.remove('fa-sun');
            themeIcon.classList.add('fa-moon');
        }
    }

    const savedTheme = localStorage.getItem('theme') || 'light';
    applyTheme(savedTheme);
    console.log("DEBUG: Tema awal diterapkan:", savedTheme);

    themeToggleBtn.addEventListener('click', function () {
        console.log("DEBUG: Tombol BERHASIL diklik!");
        const currentTheme = htmlElement.getAttribute('data-bs-theme') || 'light';
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

        applyTheme(newTheme);
        localStorage.setItem('theme', newTheme);
        console.log("DEBUG: Tema diubah dan disimpan:", newTheme);
    });
});