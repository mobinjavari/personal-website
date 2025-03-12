AOS.init({
    duration: 800,
    once: true
});

const mobileMenuButton = document.getElementById('mobile-menu-button');
const mobileMenu = document.getElementById('mobile-menu');
const menuIcon = mobileMenuButton.querySelector('.menu-icon');
const closeIcon = mobileMenuButton.querySelector('.close-icon');
const body = document.body;

function toggleMobileMenu() {
    mobileMenu.classList.toggle('hidden');
    menuIcon.classList.toggle('hidden');
    closeIcon.classList.toggle('hidden');
    body.classList.toggle('overflow-hidden');
}

mobileMenuButton.addEventListener('click', (e) => {
    e.stopPropagation();
    toggleMobileMenu();
});

document.addEventListener('click', (e) => {
    if (!mobileMenu.classList.contains('hidden') && 
        !mobileMenu.contains(e.target) && 
        !mobileMenuButton.contains(e.target)) {
        toggleMobileMenu();
    }
});

document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});

window.addEventListener('scroll', () => {
    const nav = document.querySelector('nav');
    if (window.scrollY > 0) {
        nav.classList.add('shadow-lg');
    } else {
        nav.classList.remove('shadow-lg');
    }
});

function initTheme() {
    document.body.classList.add('theme-transition');
    
    if (localStorage.theme === 'dark' || (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
        document.documentElement.classList.add('dark');
    } else {
        document.documentElement.classList.remove('dark');
    }

    setTimeout(() => {
        document.body.classList.remove('theme-transition');
    }, 300);
}

function toggleTheme() {
    document.body.classList.add('theme-transition');

    if (document.documentElement.classList.contains('dark')) {
        document.documentElement.classList.remove('dark');
        localStorage.theme = 'light';
    } else {
        document.documentElement.classList.add('dark');
        localStorage.theme = 'dark';
    }

    setTimeout(() => {
        document.body.classList.remove('theme-transition');
    }, 300);
}

initTheme();
document.getElementById('theme-toggle').addEventListener('click', toggleTheme);

function scrollToSection(sectionId) {
    const section = document.querySelector(`section[data-section="${sectionId}"]`);
    const navHeight = 64; // Height of your sticky nav
    const offset = section.offsetTop - navHeight;
    
    window.scrollTo({
        top: offset,
        behavior: 'smooth'
    });
}

document.addEventListener('DOMContentLoaded', function() {
    const sections = ['home', 'projects', 'skills', 'contact'];
    sections.forEach(section => {
        const element = document.querySelector(`[data-section="${section}"]`);
        if (element) {
            element.id = section;
        }
    });
});

// Intersection Observer for sections
const sections = document.querySelectorAll('section[data-section]');
const navButtons = document.querySelectorAll('.nav-section');

const observerOptions = {
    root: null,
    rootMargin: '-50% 0px',
    threshold: 0
};

const sectionObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            const activeSection = entry.target.getAttribute('data-section');
            updateActiveSection(activeSection);
        }
    });
}, observerOptions);

sections.forEach(section => sectionObserver.observe(section));

function updateActiveSection(sectionId) {
    navButtons.forEach(button => {
        const buttonSection = button.getAttribute('data-section');
        const indicator = button.querySelector('.active-indicator');
        
        if (buttonSection === sectionId) {
            button.classList.add('text-[#238636]', 'dark:text-[#3fb950]');
            indicator.classList.replace('scale-x-0', 'scale-x-100');
        } else {
            button.classList.remove('text-[#238636]', 'dark:text-[#3fb950]');
            indicator.classList.replace('scale-x-100', 'scale-x-0');
        }
    });
}

function animateSkillBars() {
    const skillBars = document.querySelectorAll('[data-progress]');
    
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const progress = entry.target.getAttribute('data-progress');
                entry.target.style.width = `${progress}%`;
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    skillBars.forEach(bar => observer.observe(bar));
}

document.addEventListener('DOMContentLoaded', animateSkillBars);
