// ============================================
// BRICOULI - Animations Interactives Pro
// ============================================

document.addEventListener('DOMContentLoaded', function () {

    // ============================================
    // 1. NAVBAR - Effet Scroll
    // ============================================
    const navbar = document.querySelector('.navbar');
    let lastScroll = 0;

    window.addEventListener('scroll', () => {
        const currentScroll = window.pageYOffset;

        if (currentScroll > 100) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }

        lastScroll = currentScroll;
    });

    // ============================================
    // 2. SCROLL ANIMATIONS - Intersection Observer
    // ============================================

    // Observer pour les feature cards
    const observerOptions = {
        threshold: 0.2,
        rootMargin: '0px 0px -100px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 150); // Stagger effect
            }
        });
    }, observerOptions);

    // Appliquer aux feature cards
    const featureCards = document.querySelectorAll('.feature-card');
    featureCards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(50px)';
        card.style.transition = 'all 0.8s cubic-bezier(0.16, 1, 0.3, 1)';
        observer.observe(card);
    });

    // Observer pour les steps
    const stepObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, {
        threshold: 0.3
    });

    const steps = document.querySelectorAll('.step');
    steps.forEach(step => {
        stepObserver.observe(step);
    });

    // ============================================
    // 3. PARALLAX EFFECT - Hero Background
    // ============================================
    const heroBackground = document.querySelector('.hero-background');

    if (heroBackground) {
        window.addEventListener('scroll', () => {
            const scrolled = window.pageYOffset;
            const parallaxSpeed = 0.5;
            heroBackground.style.transform = `translateY(${scrolled * parallaxSpeed}px)`;
        });
    }

    // ============================================
    // 4. SMOOTH SCROLL pour les liens d'ancre
    // ============================================
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                const offsetTop = target.offsetTop - 70; // Navbar height
                window.scrollTo({
                    top: offsetTop,
                    behavior: 'smooth'
                });
            }
        });
    });

    // ============================================
    // 5. CATEGORIES SCROLL - Indicateurs
    // ============================================
    const categoriesScroll = document.querySelector('.categories-scroll');

    if (categoriesScroll) {
        let isDown = false;
        let startX;
        let scrollLeft;

        categoriesScroll.addEventListener('mousedown', (e) => {
            isDown = true;
            categoriesScroll.style.cursor = 'grabbing';
            startX = e.pageX - categoriesScroll.offsetLeft;
            scrollLeft = categoriesScroll.scrollLeft;
        });

        categoriesScroll.addEventListener('mouseleave', () => {
            isDown = false;
            categoriesScroll.style.cursor = 'grab';
        });

        categoriesScroll.addEventListener('mouseup', () => {
            isDown = false;
            categoriesScroll.style.cursor = 'grab';
        });

        categoriesScroll.addEventListener('mousemove', (e) => {
            if (!isDown) return;
            e.preventDefault();
            const x = e.pageX - categoriesScroll.offsetLeft;
            const walk = (x - startX) * 2;
            categoriesScroll.scrollLeft = scrollLeft - walk;
        });

        // Touch events pour mobile
        let touchStartX = 0;
        let touchScrollLeft = 0;

        categoriesScroll.addEventListener('touchstart', (e) => {
            touchStartX = e.touches[0].pageX;
            touchScrollLeft = categoriesScroll.scrollLeft;
        });

        categoriesScroll.addEventListener('touchmove', (e) => {
            const touchX = e.touches[0].pageX;
            const walk = (touchStartX - touchX) * 1.5;
            categoriesScroll.scrollLeft = touchScrollLeft + walk;
        });
    }

    // ============================================
    // 6. COUNTER ANIMATION pour les statistiques
    // ============================================
    function animateCounter(element, target, duration = 2000) {
        const start = 0;
        const increment = target / (duration / 16);
        let current = start;

        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                element.textContent = target.toLocaleString();
                clearInterval(timer);
            } else {
                element.textContent = Math.floor(current).toLocaleString();
            }
        }, 16);
    }

    // Observer pour les compteurs
    const counterObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting && !entry.target.classList.contains('counted')) {
                const target = parseInt(entry.target.getAttribute('data-target'));
                animateCounter(entry.target, target);
                entry.target.classList.add('counted');
            }
        });
    }, { threshold: 0.5 });

    document.querySelectorAll('.counter').forEach(counter => {
        counterObserver.observe(counter);
    });

    // ============================================
    // 7. TYPING EFFECT pour le hero
    // ============================================
    function typeWriter(element, text, speed = 100) {
        let i = 0;
        element.textContent = '';

        function type() {
            if (i < text.length) {
                element.textContent += text.charAt(i);
                i++;
                setTimeout(type, speed);
            }
        }

        type();
    }

    // Utiliser si vous voulez un effet typing
    const typingElement = document.querySelector('.typing-effect');
    if (typingElement) {
        const text = typingElement.getAttribute('data-text');
        typeWriter(typingElement, text, 80);
    }

    // ============================================
    // 8. CURSOR EFFECT - Suiveur de souris (optionnel)
    // ============================================
    const cursor = document.createElement('div');
    cursor.classList.add('custom-cursor');
    document.body.appendChild(cursor);

    const cursorFollower = document.createElement('div');
    cursorFollower.classList.add('cursor-follower');
    document.body.appendChild(cursorFollower);

    let mouseX = 0, mouseY = 0;
    let cursorX = 0, cursorY = 0;
    let followerX = 0, followerY = 0;

    document.addEventListener('mousemove', (e) => {
        mouseX = e.clientX;
        mouseY = e.clientY;
    });

    function animateCursor() {
        // Cursor principal
        cursorX += (mouseX - cursorX) * 0.3;
        cursorY += (mouseY - cursorY) * 0.3;
        cursor.style.left = cursorX + 'px';
        cursor.style.top = cursorY + 'px';

        // Follower
        followerX += (mouseX - followerX) * 0.1;
        followerY += (mouseY - followerY) * 0.1;
        cursorFollower.style.left = followerX + 'px';
        cursorFollower.style.top = followerY + 'px';

        requestAnimationFrame(animateCursor);
    }

    // Décommenter pour activer le curseur custom
    // animateCursor();

    // Effet hover sur les éléments interactifs
    const interactiveElements = document.querySelectorAll('a, button, .feature-card, .category-card');
    interactiveElements.forEach(el => {
        el.addEventListener('mouseenter', () => {
            cursor.classList.add('cursor-hover');
            cursorFollower.classList.add('cursor-hover');
        });

        el.addEventListener('mouseleave', () => {
            cursor.classList.remove('cursor-hover');
            cursorFollower.classList.remove('cursor-hover');
        });
    });

    // ============================================
    // 9. LAZY LOADING pour les images
    // ============================================
    const imageObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.add('loaded');
                imageObserver.unobserve(img);
            }
        });
    });

    document.querySelectorAll('img[data-src]').forEach(img => {
        imageObserver.observe(img);
    });

    // ============================================
    // 10. PRELOADER - Animation de chargement
    // ============================================
    const preloader = document.querySelector('.preloader');
    if (preloader) {
        window.addEventListener('load', () => {
            setTimeout(() => {
                preloader.style.opacity = '0';
                setTimeout(() => {
                    preloader.style.display = 'none';
                }, 500);
            }, 1000);
        });
    }

    // ============================================
    // 11. SCROLL PROGRESS BAR
    // ============================================
    const progressBar = document.createElement('div');
    progressBar.classList.add('scroll-progress');
    document.body.appendChild(progressBar);

    window.addEventListener('scroll', () => {
        const windowHeight = document.documentElement.scrollHeight - document.documentElement.clientHeight;
        const scrolled = (window.pageYOffset / windowHeight) * 100;
        progressBar.style.width = scrolled + '%';
    });

    // ============================================
    // 12. ANIMATIONS AU HOVER - 3D Tilt Effect
    // ============================================
    const cards = document.querySelectorAll('.feature-card, .category-card');

    cards.forEach(card => {
        card.addEventListener('mousemove', (e) => {
            const rect = card.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            const centerX = rect.width / 2;
            const centerY = rect.height / 2;

            const rotateX = (y - centerY) / 10;
            const rotateY = (centerX - x) / 10;

            card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.05)`;
        });

        card.addEventListener('mouseleave', () => {
            card.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) scale(1)';
        });
    });

    console.log('🎉 Bricouli animations loaded successfully!');
});