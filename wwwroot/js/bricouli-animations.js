// ============================================
// BRICOULI - Animations Interactives Pro (Optimisé)
// ============================================

document.addEventListener('DOMContentLoaded', function () {

    // ============================================
    // UTILITY - Throttle fonction pour optimiser mousemove
    // ============================================
    function throttle(func, limit) {
        let inThrottle;
     return function(...args) {
            if (!inThrottle) {
     func.apply(this, args);
             inThrottle = true;
   setTimeout(() => inThrottle = false, limit);
  }
        };
    }

    // --------------------------------------------
    // Smooth scroll for anchors (ignore absolute routes)
    // --------------------------------------------
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
      const href = this.getAttribute('href');
 // If it's a route (starts with /) let the browser handle it
 if (href && href.startsWith('/')) return;
      e.preventDefault();
            const target = document.querySelector(href);
         if (target) {
       const offsetTop = target.offsetTop -70;
     window.scrollTo({
               top: offsetTop,
         behavior: 'smooth'
          });
          }
        });
    });

    // --------------------------------------------
    // Buttons that should go to pages
    // --------------------------------------------
    document.querySelectorAll('a.btn-primary, a.btn-secondary, button.btn-primary, button.btn-secondary').forEach(el => {
 el.addEventListener('click', function (e) {
 const href = el.getAttribute ? el.getAttribute('href') : null;
 // If it's an anchor handled elsewhere, ignore
 if (href && href.startsWith('#')) return;
 // For links to root-relative paths, allow default navigation
 if (href && href.startsWith('/')) return;
 });
 });

    // --------------------------------------------
    // CATEGORY / FEATURE CARD ACTIONS
    // --------------------------------------------
    function navigateToDevisWithCategory(category) {
 if (!category) { window.location.href = '/Devis'; return; }
 const url = '/Devis?category=' + encodeURIComponent(category);
 window.location.href = url;
 }

 // Category cards -> /Devis?category=...
 document.querySelectorAll('.category-card').forEach(card => {
 card.addEventListener('click', function () {
 const category = this.getAttribute('data-category');
 navigateToDevisWithCategory(category);
 });
 card.addEventListener('keydown', function (e) {
 if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); const category = this.getAttribute('data-category'); navigateToDevisWithCategory(category); }
 });
 });

 // Feature cards -> /FindProfessional (only on home feature cards)
 document.querySelectorAll('.feature-card[data-action="find"]').forEach(card => {
 card.addEventListener('click', function () { window.location.href = '/FindProfessional'; });
 card.addEventListener('keydown', function (e) { if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); window.location.href = '/FindProfessional'; } });
 });

 // Ensure hero CTA buttons point to the new pages (in case custom JS-created buttons exist)
 const heroFind = document.querySelectorAll('a[aria-label="Trouver un professionnel"]');
 heroFind.forEach(a => a.setAttribute('href', '/FindProfessional'));
 const heroBecome = document.querySelectorAll('a[aria-label="Devenir prestataire"]');
 heroBecome.forEach(a => a.setAttribute('href', '/BecomeProvider'));
 const contactBtns = document.querySelectorAll('a[aria-label="Contactez-nous"], a[aria-label="Demander un devis"]');
 contactBtns.forEach(a => {
 if (a.textContent && a.textContent.toLowerCase().includes('devis')) a.setAttribute('href', '/Devis'); else a.setAttribute('href', '/ContactUs');
 });

    // ============================================
    //1. NAVBAR - Effet Scroll
    // ============================================
    const navbar = document.querySelector('.navbar');

    window.addEventListener('scroll', throttle(() => {
        const currentScroll = window.pageYOffset;

    if (currentScroll >100) {
      navbar.classList.add('scrolled');
     } else {
    navbar.classList.remove('scrolled');
 }

    },100));

    // ============================================
    //2. SCROLL ANIMATIONS - Intersection Observer
    // ============================================

    // Observer pour les feature cards
    const observerOptions = {
    threshold:0.2,
      rootMargin: '0px 0px -100px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
      requestAnimationFrame(() => {
      setTimeout(() => {
        entry.target.style.opacity = '1';
       entry.target.style.transform = 'translateY(0)';
          }, index *150); // Stagger effect
      });
    }
        });
    }, observerOptions);

    // Appliquer aux feature cards
    const featureCards = document.querySelectorAll('.feature-card');
    featureCards.forEach(card => {
   card.style.opacity = '0';
        card.style.transform = 'translateY(50px)';
 card.style.transition = 'all 0.8s cubic-bezier(0.16,1,0.3,1)';
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
        threshold:0.3
    });

    const steps = document.querySelectorAll('.step');
    steps.forEach(step => {
        stepObserver.observe(step);
    });

    // ============================================
    //3. PARALLAX EFFECT - Hero Background (Optimisé)
    // ============================================
    const heroBackground = document.querySelector('.hero-background');

    if (heroBackground) {
        // Utiliser CSS instead de JavaScript pour les animations
        // Le parallax est maintenant gérée par CSS animation pour meilleure performance
        window.addEventListener('scroll', throttle(() => {
    const scrolled = window.pageYOffset;
   const parallaxSpeed =0.5;
      heroBackground.style.transform = `translateY(${scrolled * parallaxSpeed}px)`;
        },16)); // ~60fps
    }

    // ============================================
    //4. SMOOTH SCROLL pour les liens d'ancre
    // ============================================
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
      const href = this.getAttribute('href');
 // If it's a route (starts with /) let the browser handle it
 if (href && href.startsWith('/')) return;
      e.preventDefault();
            const target = document.querySelector(href);
         if (target) {
       const offsetTop = target.offsetTop -70; // Navbar height
     window.scrollTo({
               top: offsetTop,
         behavior: 'smooth'
          });
          }
        });
    });

    // ============================================
    //5. CATEGORIES SCROLL - Indicateurs
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

     categoriesScroll.addEventListener('mousemove', throttle((e) => {
            if (!isDown) return;
            e.preventDefault();
            const x = e.pageX - categoriesScroll.offsetLeft;
          const walk = (x - startX) *2;
            categoriesScroll.scrollLeft = scrollLeft - walk;
        },16));

  // Touch events pour mobile (déjà optimisé)
        let touchStartX =0;
        let touchScrollLeft =0;

        categoriesScroll.addEventListener('touchstart', (e) => {
     touchStartX = e.touches[0].pageX;
            touchScrollLeft = categoriesScroll.scrollLeft;
        });

        categoriesScroll.addEventListener('touchmove', (e) => {
  const touchX = e.touches[0].pageX;
         const walk = (touchStartX - touchX) *1.5;
 categoriesScroll.scrollLeft = touchScrollLeft + walk;
        });
    }

    // ============================================
    //6. COUNTER ANIMATION - Optimisé avec requestAnimationFrame
    // ============================================
    function animateCounter(element, target, duration =2000) {
 const start =0;
        let current = start;
        const startTime = performance.now();

        function updateCounter(currentTime) {
            const elapsed = currentTime - startTime;
     const progress = Math.min(elapsed / duration,1);
            
     // Easing function pour animation plus fluide
    const easeProgress = progress <0.5
          ?2 * progress * progress
     : -1 + (4 -2 * progress) * progress;
        
            current = easeProgress * target;

    if (progress >=1) {
        element.textContent = target.toLocaleString();
return;
 } else {
        element.textContent = Math.floor(current).toLocaleString();
   requestAnimationFrame(updateCounter);
         }
        }

        requestAnimationFrame(updateCounter);
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
    }, { threshold:0.5 });

    document.querySelectorAll('.counter').forEach(counter => {
        counterObserver.observe(counter);
    });

    // ============================================
    //7. TYPING EFFECT pour le hero
 // ============================================
    function typeWriter(element, text, speed =100) {
     let i =0;
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
        typeWriter(typingElement, text,80);
    }

  // ============================================
    //8. CURSOR EFFECT - Suiveur de souris (Optimisé avec throttle)
    // ============================================
    const cursor = document.createElement('div');
    cursor.classList.add('custom-cursor');
    document.body.appendChild(cursor);

    const cursorFollower = document.createElement('div');
    cursorFollower.classList.add('cursor-follower');
    document.body.appendChild(cursorFollower);

    let mouseX =0, mouseY =0;
  let cursorX =0, cursorY =0;
    let followerX =0, followerY =0;
    let animationFrameId = null;

    document.addEventListener('mousemove', throttle((e) => {
        mouseX = e.clientX;
mouseY = e.clientY;
    },16)); // ~60fps

    function animateCursor() {
        // Cursor principal
        cursorX += (mouseX - cursorX) *0.3;
        cursorY += (mouseY - cursorY) *0.3;
        cursor.style.left = cursorX + 'px';
        cursor.style.top = cursorY + 'px';

 // Follower
        followerX += (mouseX - followerX) *0.1;
        followerY += (mouseY - followerY) *0.1;
        cursorFollower.style.left = followerX + 'px';
   cursorFollower.style.top = followerY + 'px';

        animationFrameId = requestAnimationFrame(animateCursor);
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
    //9. LAZY LOADING pour les images
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
    //10. PRELOADER - Animation de chargement
    // ============================================
    const preloader = document.querySelector('.preloader');
    if (preloader) {
        window.addEventListener('load', () => {
            setTimeout(() => {
  preloader.style.opacity = '0';
      preloader.style.pointerEvents = 'none';
  setTimeout(() => {
          preloader.style.display = 'none';
                }, 500);
            }, 800);
        });
  }

    // ============================================
    //11. SCROLL PROGRESS BAR (Optimisé)
    // ============================================
    const progressBar = document.createElement('div');
    progressBar.classList.add('scroll-progress');
    document.body.appendChild(progressBar);

    window.addEventListener('scroll', throttle(() => {
   const windowHeight = document.documentElement.scrollHeight - document.documentElement.clientHeight;
        const scrolled = (window.pageYOffset / windowHeight) *100;
        progressBar.style.width = scrolled + '%';
    },16));

    // ============================================
    //12. ANIMATIONS AU HOVER -3D Tilt Effect (Optimisé)
    // ============================================
    const cards = document.querySelectorAll('.feature-card, .category-card');

    cards.forEach(card => {
        card.addEventListener('mousemove', throttle((e) => {
       const rect = card.getBoundingClientRect();
            const x = e.clientX - rect.left;
     const y = e.clientY - rect.top;

      const centerX = rect.width /2;
          const centerY = rect.height /2;

        const rotateX = (y - centerY) /10;
        const rotateY = (centerX - x) /10;

            card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.05)`;
        },16));

  card.addEventListener('mouseleave', () => {
  card.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) scale(1)';
        });
    });

 // ============================================
    // BUTTON INTERACTIONS - Ripple Effect (Optimisé)
    // ============================================
    const buttons = document.querySelectorAll('.btn-primary, .btn-secondary');

    buttons.forEach(button => {
    button.addEventListener('click', function(e) {
        // Ripple effect
            this.classList.add('ripple');

      setTimeout(() => {
                this.classList.remove('ripple');
      }, 600);

        // Navigation based on href
      const href = this.getAttribute('href');
if (href && href.startsWith('#')) {
    e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
   const offsetTop = target.offsetTop -70;
    window.scrollTo({
  top: offsetTop,
            behavior: 'smooth'
       });
    }
  }

 // Visual feedback
    this.style.transform = 'scale(0.98)';
    setTimeout(() => {
       this.style.transform = '';
            }, 150);
        });

        // Enhanced hover state
        button.addEventListener('mouseenter', function() {
            this.style.transition = 'all0.3s var(--ease-out-expo)';
      });
    });

    // ============================================
    // CATEGORY / FEATURE CARD ACTIONS
    // - Click on category navigates to /Devis with category querystring
    // - Click on feature card opens /Devis
    // ============================================
    function navigateToDevisWithCategory(category) {
 if (!category) {
 window.location.href = '/Devis';
 return;
 }
 const url = '/Devis?category=' + encodeURIComponent(category);
 window.location.href = url;
 }

 document.querySelectorAll('.category-card').forEach(card => {
 card.addEventListener('click', function () {
 const category = this.getAttribute('data-category');
 navigateToDevisWithCategory(category);
 });

 card.addEventListener('keydown', function (e) {
 if (e.key === 'Enter' || e.key === ' ') {
 e.preventDefault();
 const category = this.getAttribute('data-category');
 navigateToDevisWithCategory(category);
 }
 });
 });

 document.querySelectorAll('.feature-card[data-action="devis"]').forEach(card => {
 card.addEventListener('click', function () {
 navigateToDevisWithCategory('');
 });

 card.addEventListener('keydown', function (e) {
 if (e.key === 'Enter' || e.key === ' ') {
 e.preventDefault();
 navigateToDevisWithCategory('');
 }
 });
 });

    // ============================================
    // MODAL DEVIS - Complete implementation
    // ============================================
    const modal = document.getElementById('devisModal');
    const openBtn = document.getElementById('openDevisModal');
    const closeBtn = document.getElementById('closeDevisModal');
    const closeBtn2 = document.getElementById('closeDevisModal2');
    const overlay = modal ? modal.querySelector('.modal-overlay') : null;
    const devisForm = document.getElementById('devisForm');

    if (openBtn) {
 openBtn.addEventListener('click', () => {
 if (modal) {
 modal.classList.add('show');
 document.documentElement.style.overflow = 'hidden';
 const firstInput = modal.querySelector('.form-input');
 if (firstInput) {
 setTimeout(() => firstInput.focus(),80);
 }
 }
 });
 }

 function closeModal() {
 if (modal) {
 modal.classList.remove('show');
 document.documentElement.style.overflow = '';
 }
 }

 if (closeBtn) {
 closeBtn.addEventListener('click', closeModal);
 }

 if (closeBtn2) {
 closeBtn2.addEventListener('click', closeModal);
 }

 if (overlay) {
 overlay.addEventListener('click', closeModal);
 }

 document.addEventListener('keydown', (e) => {
 if (e.key === 'Escape' && modal && modal.classList.contains('show')) {
 closeModal();
 }
 });

// Devis form submission
// Let the MVC form post to /Devis to avoid hitting a removed endpoint.

    // ============================================
    // OUVERTURE POPUP POUR DEMANDER UN DEVIS
    // ============================================
    const devisPopupBtn = document.getElementById('openDevisPopup');
    if (devisPopupBtn) {
 devisPopupBtn.addEventListener('click', function (e) {
 e.preventDefault();
 window.open('/Devis', 'DemanderUnDevis', 'width=700,height=800,menubar=no,toolbar=no,location=no,status=no,resizable=yes,scrollbars=yes');
 });
    }

    console.log('✨ Bricouli optimized animations loaded successfully!');
});




