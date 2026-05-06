/* ===== نظام التسجيل المدني - JavaScript ===== */
/* Pure Vanilla JS - No React, No jQuery dependency */

// ===== ADMIN CREDENTIALS =====
var ADMIN_EMAIL = 'admin@crs.gov.eg';
var ADMIN_PASSWORD = 'Admin@2025';
var ADMIN_NAME = 'محمد أحمد';

// ===== NAVBAR MOBILE TOGGLE =====
document.addEventListener('DOMContentLoaded', function() {
  // Initialize Bootstrap navbar collapse
  var navbarToggler = document.querySelector('.navbar-toggler');
  var navbarCollapse = document.querySelector('.navbar-collapse');

  if (navbarToggler && navbarCollapse) {
    navbarToggler.addEventListener('click', function() {
      var bsCollapse = bootstrap.Collapse.getOrCreateInstance(navbarCollapse, {toggle: false});
      bsCollapse.toggle();
    });

    // Close navbar when clicking a link on mobile
    var navLinks = navbarCollapse.querySelectorAll('.nav-link');
    navLinks.forEach(function(link) {
      link.addEventListener('click', function() {
        if (navbarCollapse.classList.contains('show')) {
          var bsCollapse = bootstrap.Collapse.getInstance(navbarCollapse);
          if (bsCollapse) bsCollapse.hide();
        }
      });
    });
  }

  // ===== SCROLL TO TOP =====
  var scrollTopBtn = document.querySelector('.scroll-top-btn');
  if (scrollTopBtn) {
    window.addEventListener('scroll', function() {
      if (window.scrollY > 400) {
        scrollTopBtn.classList.add('show');
      } else {
        scrollTopBtn.classList.remove('show');
      }
    });
    scrollTopBtn.addEventListener('click', function() {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  }

  // ===== FADE UP ANIMATIONS =====
  var fadeElements = document.querySelectorAll('.fade-up');
  function checkFade() {
    fadeElements.forEach(function(el) {
      var rect = el.getBoundingClientRect();
      if (rect.top < window.innerHeight - 100) {
        el.classList.add('visible');
      }
    });
  }
  window.addEventListener('scroll', checkFade, { passive: true });
  checkFade();

  // ===== SMOOTH SCROLL =====
  document.querySelectorAll('a[href^="#"]').forEach(function(anchor) {
    anchor.addEventListener('click', function(e) {
      var target = document.querySelector(this.getAttribute('href'));
      if (target) {
        e.preventDefault();
        target.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    });
  });

  // ===== ACTIVE NAV LINK =====
  var currentPage = window.location.pathname.split('/').pop() || 'index.html';
  document.querySelectorAll('.gov-navbar .nav-link').forEach(function(link) {
    var href = link.getAttribute('href');
    if (href === currentPage || (currentPage === '' && href === 'index.html')) {
      link.classList.add('active');
    }
  });

  // ===== UPDATE NAVBAR BASED ON LOGIN STATUS =====
  updateNavbarForLoginStatus();
});

// ===== UPDATE NAVBAR BASED ON LOGIN STATUS =====
function updateNavbarForLoginStatus() {
  var user = getUser();
  var navButtons = document.querySelector('.nav-buttons');
  if (!navButtons) return;

  if (user && user.loggedIn) {
    if (user.role === 'admin') {
      navButtons.innerHTML =
        '<span class="d-none d-md-inline" style="color:rgba(255,255,255,0.85);font-weight:600;font-size:0.9rem;">' +
        '  <i class="bi bi-shield-check ms-1" style="color:var(--gov-red);"></i> أدمن: ' + user.name +
        '</span>' +
        '<a href="admin.html" class="btn btn-gov-gold btn-sm"><i class="bi bi-speedometer2 ms-1"></i> لوحة التحكم</a>' +
        '<button class="btn btn-gov-outline btn-sm" onclick="handleLogout()">' +
        '  <i class="bi bi-box-arrow-left ms-1"></i> خروج' +
        '</button>';
    } else {
      navButtons.innerHTML =
        '<span class="d-none d-md-inline" style="color:rgba(255,255,255,0.85);font-weight:600;font-size:0.9rem;">' +
        '  <i class="bi bi-person-circle ms-1"></i> ' + user.name +
        '</span>' +
        '<a href="dashboard.html" class="btn btn-gov-gold btn-sm"><i class="bi bi-speedometer2 ms-1"></i> لوحة التحكم</a>' +
        '<button class="btn btn-gov-outline btn-sm" onclick="handleLogout()">' +
        '  <i class="bi bi-box-arrow-left ms-1"></i> خروج' +
        '</button>';
    }
  }
}

// ===== LOGIN HANDLER =====
function handleLogin(e) {
  e.preventDefault();
  var form = e.target;
  var email = form.querySelector('[name="email"]').value.trim();
  var password = form.querySelector('[name="password"]').value;

  if (!email || !password) {
    showAlert('يرجى ملء جميع الحقول', 'danger');
    return false;
  }

  // Check admin credentials
  if (email === ADMIN_EMAIL && password === ADMIN_PASSWORD) {
    localStorage.setItem('crs_user', JSON.stringify({
      name: ADMIN_NAME,
      email: email,
      role: 'admin',
      loggedIn: true
    }));
    showAlert('تم تسجيل الدخول كأدمن بنجاح', 'success');
    setTimeout(function() {
      window.location.href = 'admin.html';
    }, 800);
    return false;
  }

  // Check if it's a registered citizen
  var users = JSON.parse(localStorage.getItem('crs_users') || '[]');
  var foundUser = users.find(function(u) {
    return (u.email === email || u.nationalId === email) && u.password === password;
  });

  if (foundUser) {
    localStorage.setItem('crs_user', JSON.stringify({
      name: foundUser.name,
      email: foundUser.email,
      nationalId: foundUser.nationalId,
      phone: foundUser.phone,
      role: 'citizen',
      loggedIn: true
    }));
    showAlert('تم تسجيل الدخول بنجاح', 'success');
    setTimeout(function() {
      window.location.href = 'dashboard.html';
    }, 800);
    return false;
  }

  showAlert('بيانات الدخول غير صحيحة. تأكد من البريد الإلكتروني وكلمة المرور', 'danger');
  return false;
}

// ===== REGISTER HANDLER =====
function handleRegister(e) {
  e.preventDefault();
  var form = e.target;
  var name = form.querySelector('[name="fullname"]').value.trim();
  var nationalId = form.querySelector('[name="nationalId"]').value.trim();
  var email = form.querySelector('[name="email"]').value.trim();
  var phone = form.querySelector('[name="phone"]') ? form.querySelector('[name="phone"]').value.trim() : '';
  var password = form.querySelector('[name="password"]').value;
  var confirmPassword = form.querySelector('[name="confirmPassword"]').value;

  if (!name || !nationalId || !email || !password) {
    showAlert('يرجى ملء جميع الحقول المطلوبة', 'danger');
    return false;
  }

  if (nationalId.length !== 14) {
    showAlert('الرقم القومي يجب أن يكون 14 رقم', 'danger');
    return false;
  }

  if (password !== confirmPassword) {
    showAlert('كلمة المرور غير متطابقة', 'danger');
    return false;
  }

  if (password.length < 6) {
    showAlert('كلمة المرور يجب أن تكون 6 أحرف على الأقل', 'danger');
    return false;
  }

  // Check if user already exists
  var users = JSON.parse(localStorage.getItem('crs_users') || '[]');
  var existingUser = users.find(function(u) { return u.email === email || u.nationalId === nationalId; });
  if (existingUser) {
    showAlert('يوجد حساب مسجل بهذا البريد أو الرقم القومي بالفعل', 'danger');
    return false;
  }

  // Save user
  var newUser = {
    name: name,
    nationalId: nationalId,
    email: email,
    phone: phone,
    password: password,
    role: 'citizen',
    registeredAt: new Date().toISOString()
  };
  users.push(newUser);
  localStorage.setItem('crs_users', JSON.stringify(users));

  // Auto login
  localStorage.setItem('crs_user', JSON.stringify({
    name: name,
    email: email,
    nationalId: nationalId,
    phone: phone,
    role: 'citizen',
    loggedIn: true
  }));

  showAlert('تم التسجيل بنجاح! جاري تحويلك...', 'success');
  setTimeout(function() {
    window.location.href = 'dashboard.html';
  }, 1500);
  return false;
}

// ===== LOGOUT =====
function handleLogout() {
  localStorage.removeItem('crs_user');
  window.location.href = 'index.html';
}

// ===== CHECK LOGIN STATUS =====
function checkLogin() {
  var user = JSON.parse(localStorage.getItem('crs_user') || 'null');
  return user && user.loggedIn;
}

function getUser() {
  return JSON.parse(localStorage.getItem('crs_user') || 'null');
}

// ===== REDIRECT IF NOT LOGGED IN =====
function requireLogin() {
  if (!checkLogin()) {
    window.location.href = 'login.html';
    return false;
  }
  return true;
}

function requireAdmin() {
  var user = getUser();
  if (!user || !user.loggedIn || user.role !== 'admin') {
    showAlert('ليس لديك صلاحية الوصول لهذه الصفحة', 'danger');
    setTimeout(function() {
      window.location.href = 'login.html';
    }, 1000);
    return false;
  }
  return true;
}

// ===== SHOW ALERT =====
function showAlert(message, type) {
  var alertDiv = document.createElement('div');
  alertDiv.className = 'alert alert-gov-' + (type || 'info') + ' alert-dismissible fade show';
  alertDiv.style.cssText = 'position:fixed;top:20px;left:50%;transform:translateX(-50%);z-index:9999;min-width:300px;max-width:90%;';
  alertDiv.innerHTML = message +
    '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>';
  document.body.appendChild(alertDiv);

  setTimeout(function() {
    alertDiv.remove();
  }, 4000);
}

// ===== SERVICE WORKFLOW DEFINITIONS =====
// Each service has its own realistic workflow steps
var serviceWorkflows = {
  license: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة المستندات المقدمة', type: 'admin' },
      { id: 'medical_pending', name: 'بانتظار الكشف الطبي', description: 'سيتم تحديد موعد الكشف الطبي', type: 'admin_assign' },
      { id: 'medical_done', name: 'تم الكشف الطبي', description: 'تم إجتياز الكشف الطبي بنجاح', type: 'admin' },
      { id: 'theory_pending', name: 'بانتظار الاختبار النظري', description: 'سيتم تحديد موعد الاختبار النظري', type: 'admin_assign' },
      { id: 'theory_done', name: 'تم الاختبار النظري', description: 'تم اجتياز الاختبار النظري بنجاح', type: 'admin' },
      { id: 'practical_pending', name: 'بانتظار الاختبار العملي', description: 'سيتم تحديد موعد الاختبار العملي', type: 'admin_assign' },
      { id: 'practical_done', name: 'تم الاختبار العملي', description: 'تم اجتياز الاختبار العملي بنجاح', type: 'admin' },
      { id: 'payment_pending', name: 'بانتظار الدفع', description: 'يرجى سداد الرسوم المقررة', type: 'citizen' },
      { id: 'payment_done', name: 'تم الدفع', description: 'تم سداد الرسوم بنجاح', type: 'auto' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار الرخصة بنجاح', type: 'admin' }
    ]
  },
  passport: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة المستندات المقدمة', type: 'admin' },
      { id: 'payment_pending', name: 'بانتظار الدفع', description: 'يرجى سداد الرسوم المقررة', type: 'citizen' },
      { id: 'payment_done', name: 'تم الدفع', description: 'تم سداد الرسوم بنجاح', type: 'auto' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار الجواز بنجاح', type: 'admin' }
    ]
  },
  nationalid: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة المستندات المقدمة', type: 'admin' },
      { id: 'payment_pending', name: 'بانتظار الدفع', description: 'يرجى سداد الرسوم المقررة', type: 'citizen' },
      { id: 'payment_done', name: 'تم الدفع', description: 'تم سداد الرسوم بنجاح', type: 'auto' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار البطاقة بنجاح', type: 'admin' }
    ]
  },
  birth: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة الطلب', type: 'admin' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار الشهادة بنجاح', type: 'admin' }
    ]
  },
  death: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة الطلب', type: 'admin' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار الشهادة بنجاح', type: 'admin' }
    ]
  },
  marriage: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة الطلب', type: 'admin' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار القسيمة بنجاح', type: 'admin' }
    ]
  },
  divorce: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة الطلب', type: 'admin' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار القسيمة بنجاح', type: 'admin' }
    ]
  },
  family: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة الطلب', type: 'admin' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار القيد بنجاح', type: 'admin' }
    ]
  },
  individual: {
    steps: [
      { id: 'submitted', name: 'تم التقديم', description: 'تم استلام طلبك بنجاح', type: 'auto' },
      { id: 'doc_review', name: 'مراجعة المستندات', description: 'جاري مراجعة الطلب', type: 'admin' },
      { id: 'issued', name: 'تم الإصدار', description: 'تم إصدار القيد بنجاح', type: 'admin' }
    ]
  }
};

// ===== APPLICATION DATA =====
var applicationData = {};

// Service definitions
var services = {
  birth: {
    id: 'birth',
    title: 'شهادة الميلاد',
    icon: 'bi-file-earmark-medical',
    color: 'birth',
    types: [
      { id: 'extract', name: 'مستخرج شهادة ميلاد', price: '63 ج.م', docs: ['الرقم القومي + اسم الأم الثلاثي'] },
      { id: 'new', name: 'تسجيل ميلاد جديد', price: 'مجاناً', docs: ['إشعار ولادة من المستشفى', 'بطاقة ولي الأمر'] }
    ]
  },
  death: {
    id: 'death',
    title: 'شهادة الوفاة',
    icon: 'bi-file-earmark-minus',
    color: 'death',
    types: [
      { id: 'extract', name: 'إصدار شهادة وفاة مميكنة', price: '63 ج.م', docs: ['تقرير طبي', 'إشعار الوفاة من المستشفى'] }
    ]
  },
  nationalid: {
    id: 'nationalid',
    title: 'بطاقة الرقم القومي',
    icon: 'bi-person-badge',
    color: 'nationalid',
    types: [
      { id: 'first', name: 'أول مرة (سن 16)', price: '75 ج.م', docs: ['شهادة ميلاد مميكنة', 'عدد 2 صورة شخصية', 'قيد عائلي أو إثبات إقامة'] },
      { id: 'renewal', name: 'تجديد', price: '75 ج.م', docs: ['البطاقة القديمة', 'عدد 2 صورة شخصية'] },
      { id: 'lost', name: 'بدل فاقد', price: '315 ج.م', docs: ['محضر فقدان', 'عدد 2 صورة شخصية'] },
      { id: 'damaged', name: 'بدل تالف', price: '265 ج.م', docs: ['البطاقة التالفة', 'عدد 2 صورة شخصية'] }
    ]
  },
  passport: {
    id: 'passport',
    title: 'جواز السفر',
    icon: 'bi-airplane',
    color: 'passport',
    types: [
      { id: 'first', name: 'أول مرة', price: '500 ج.م', docs: ['بطاقة قومي سارية', 'عدد 4 صور شخصية', 'شهادة موقف التجنيد (للذكور)'] },
      { id: 'renewal', name: 'تجديد', price: '500 ج.م', docs: ['الجواز القديم', 'بطاقة قومي سارية', 'عدد 4 صور شخصية'] },
      { id: 'lost', name: 'بدل فاقد', price: '600 ج.م', docs: ['محضر فقدان', 'نفس مستندات أول مرة'] },
      { id: 'damaged', name: 'بدل تالف', price: '500 ج.م', docs: ['الجواز التالف', 'بطاقة قومي سارية'] }
    ]
  },
  license: {
    id: 'license',
    title: 'رخصة القيادة',
    icon: 'bi-car-front',
    color: 'license',
    types: [
      { id: 'first', name: 'أول مرة (خاصة)', price: '1,140 ج.م', docs: ['بطاقة قومي + مؤهل دراسي', 'عدد 2 صورة شخصية', 'إثبات إقامة'] },
      { id: 'renewal', name: 'تجديد', price: '1,105 ج.م', docs: ['الرخصة القديمة', 'شهادة براءة ذمة'] },
      { id: 'lost', name: 'بدل فاقد', price: '265 ج.م', docs: ['بطاقة قومي', 'محضر فقدان', 'شهادة براءة ذمة'] },
      { id: 'damaged', name: 'بدل تالف', price: '215 ج.م', docs: ['بطاقة قومي', 'الرخصة التالفة', 'شهادة براءة ذمة'] }
    ]
  },
  marriage: {
    id: 'marriage',
    title: 'قسيمة الزواج',
    icon: 'bi-heart',
    color: 'marriage',
    types: [
      { id: 'extract', name: 'مستخرج قسيمة زواج', price: '63 ج.م', docs: ['بيانات الزوج والزوجة (ثلاثي-رباعي)', 'تاريخ الزواج'] }
    ]
  },
  divorce: {
    id: 'divorce',
    title: 'قسيمة الطلاق',
    icon: 'bi-scissors',
    color: 'divorce',
    types: [
      { id: 'extract', name: 'مستخرج قسيمة طلاق', price: '63 ج.م', docs: ['بيانات الزوج والزوجة', 'تاريخ الطلاق'] }
    ]
  },
  family: {
    id: 'family',
    title: 'قيد عائلي',
    icon: 'bi-people',
    color: 'family',
    types: [
      { id: 'extract', name: 'مستخرج قيد عائلي', price: '63 ج.م', docs: ['الرقم القومي', 'اسم رب الأسرة'] }
    ]
  },
  individual: {
    id: 'individual',
    title: 'قيد فردي',
    icon: 'bi-person-lines-fill',
    color: 'individual',
    types: [
      { id: 'extract', name: 'مستخرج قيد فردي', price: '63 ج.م', docs: ['الرقم القومي'] }
    ]
  }
};

// ===== GOVERNORATES =====
var governorates = [
  'القاهرة', 'الجيزة', 'الإسكندرية', 'البحيرة', 'الشرقية', 'الدقهلية',
  'القليوبية', 'المنوفية', 'الغربية', 'كفر الشيخ', 'دمياط', 'بورسعيد',
  'الإسماعيلية', 'السويس', 'شمال سيناء', 'جنوب سيناء', 'البحر الأحمر',
  'الوادي الجديد', 'مطروح', 'الفيوم', 'بني سويف', 'المنيا', 'أسيوط',
  'سوهاج', 'قنا', 'الأقصر', 'أسوان'
];

function populateGovernorates() {
  var select = document.getElementById('governorate');
  if (!select) return;
  select.innerHTML = '<option value="">اختر المحافظة</option>';
  governorates.forEach(function(gov) {
    var option = document.createElement('option');
    option.value = gov;
    option.textContent = gov;
    select.appendChild(option);
  });
}

// ===== GET URL PARAMETER =====
function getUrlParam(param) {
  var urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(param);
}

// ===== GET APPLICATION STATUS INFO =====
function getStatusInfo(serviceId, stepId) {
  var workflow = serviceWorkflows[serviceId];
  if (!workflow) return { name: stepId, color: '#6c757d' };

  var step = workflow.steps.find(function(s) { return s.id === stepId; });
  if (!step) return { name: stepId, color: '#6c757d' };

  var colorMap = {
    'submitted': '#1565c0',
    'doc_review': '#c8a415',
    'medical_pending': '#e65100',
    'medical_done': '#2e7d32',
    'theory_pending': '#e65100',
    'theory_done': '#2e7d32',
    'practical_pending': '#e65100',
    'practical_done': '#2e7d32',
    'payment_pending': '#e65100',
    'payment_done': '#2e7d32',
    'issued': '#1a7a3a',
    'rejected': '#c41e3a'
  };

  return {
    name: step.name,
    description: step.description,
    color: colorMap[stepId] || '#6c757d',
    type: step.type
  };
}

// ===== GET APPLICATION PROGRESS =====
function getApplicationProgress(serviceId, currentStepId) {
  var workflow = serviceWorkflows[serviceId];
  if (!workflow) return 0;

  var idx = workflow.steps.findIndex(function(s) { return s.id === currentStepId; });
  if (idx === -1) return 0;
  return Math.round(((idx + 1) / workflow.steps.length) * 100);
}

// ===== GET NEXT STEP FOR SERVICE =====
function getNextStep(serviceId, currentStepId) {
  var workflow = serviceWorkflows[serviceId];
  if (!workflow) return null;

  var idx = workflow.steps.findIndex(function(s) { return s.id === currentStepId; });
  if (idx === -1 || idx >= workflow.steps.length - 1) return null;
  return workflow.steps[idx + 1];
}

// ===== INITIALIZE APPLY PAGE =====
function initApplyPage() {
  var serviceId = getUrlParam('service');
  if (!serviceId || !services[serviceId]) {
    document.getElementById('serviceSelection').style.display = 'block';
    document.getElementById('applicationForm').style.display = 'none';
    return;
  }

  var service = services[serviceId];
  applicationData.serviceId = serviceId;
  applicationData.serviceName = service.title;

  document.getElementById('serviceSelection').style.display = 'none';
  document.getElementById('applicationForm').style.display = 'block';

  // Update header
  var headerIcon = document.getElementById('serviceHeaderIcon');
  var headerTitle = document.getElementById('serviceHeaderTitle');
  var headerIconContainer = document.querySelector('#applicationForm .detail-icon');

  if (headerIconContainer) {
    headerIconContainer.className = 'detail-icon color-' + service.color;
  }
  if (headerIcon) headerIcon.className = 'bi ' + service.icon;
  if (headerTitle) headerTitle.textContent = service.title;

  // Build type selection
  var typesContainer = document.getElementById('serviceTypes');
  if (typesContainer) {
    typesContainer.innerHTML = '';
    service.types.forEach(function(type) {
      var col = document.createElement('div');
      col.className = 'col-md-6 col-lg-3 mb-3';
      col.innerHTML =
        '<div class="service-type-card" data-type-id="' + type.id + '" onclick="selectServiceType(\'' + type.id + '\')">' +
        '  <div class="type-icon"><i class="bi ' + service.icon + '"></i></div>' +
        '  <div class="type-name">' + type.name + '</div>' +
        '  <div class="type-price">' + type.price + '</div>' +
        '</div>';
      typesContainer.appendChild(col);
    });
  }

  showStep(1);
}

// ===== SELECT SERVICE TYPE =====
function selectServiceType(typeId) {
  applicationData.typeId = typeId;
  var service = services[applicationData.serviceId];
  var selectedType = service.types.find(function(t) { return t.id === typeId; });
  applicationData.typeName = selectedType.name;
  applicationData.price = selectedType.price;

  document.querySelectorAll('.service-type-card').forEach(function(card) {
    card.classList.remove('selected');
  });
  var selectedCard = document.querySelector('.service-type-card[data-type-id="' + typeId + '"]');
  if (selectedCard) selectedCard.classList.add('selected');

  // Build required docs list
  var docsContainer = document.getElementById('requiredDocs');
  if (docsContainer) {
    docsContainer.innerHTML = '';
    selectedType.docs.forEach(function(doc) {
      var docItem = document.createElement('div');
      docItem.className = 'required-doc';
      docItem.innerHTML =
        '<i class="bi bi-check-circle-fill doc-check"></i>' +
        '<span>' + doc + '</span>';
      docsContainer.appendChild(docItem);
    });

    var docsSection = document.getElementById('requiredDocsContainer');
    if (docsSection) docsSection.style.display = 'block';
  }

  // Add upload areas
  var uploadContainer = document.getElementById('uploadDocs');
  if (uploadContainer) {
    uploadContainer.innerHTML = '';
    selectedType.docs.forEach(function(doc, idx) {
      var uploadDiv = document.createElement('div');
      uploadDiv.className = 'col-md-6 mb-3';
      uploadDiv.innerHTML =
        '<label class="form-label fw-bold" style="font-size:0.85rem;">' + doc + '</label>' +
        '<div class="doc-upload-area" id="upload-' + idx + '" onclick="simulateUpload(' + idx + ')">' +
        '  <i class="bi bi-cloud-arrow-up"></i>' +
        '  <div style="font-size:0.85rem; color:var(--gov-text-light);">اضغط لرفع المستند</div>' +
        '  <div style="font-size:0.75rem; color:var(--gov-text-light);">PDF, JPG, PNG - حد أقصى 5MB</div>' +
        '</div>';
      uploadContainer.appendChild(uploadDiv);
    });
  }

  var nextBtn = document.getElementById('nextBtn1');
  if (nextBtn) nextBtn.disabled = false;
}

// ===== SIMULATE FILE UPLOAD =====
function simulateUpload(idx) {
  var uploadArea = document.getElementById('upload-' + idx);
  if (uploadArea && !uploadArea.classList.contains('uploaded')) {
    uploadArea.classList.add('uploaded');
    uploadArea.innerHTML =
      '<i class="bi bi-check-circle-fill" style="font-size:2rem; color:var(--gov-green);"></i>' +
      '<div style="font-size:0.85rem; color:var(--gov-green); font-weight:600;">تم رفع المستند بنجاح</div>' +
      '<div style="font-size:0.75rem; color:var(--gov-text-light);">document_' + (idx + 1) + '.pdf</div>';
  }
}

// ===== FORM STEPS =====
var currentStep = 1;
var totalSteps = 4;

function showStep(step) {
  currentStep = step;

  document.querySelectorAll('.form-step').forEach(function(s) {
    s.classList.remove('active');
  });

  var currentStepEl = document.getElementById('formStep' + step);
  if (currentStepEl) currentStepEl.classList.add('active');

  for (var i = 1; i <= totalSteps; i++) {
    var dot = document.getElementById('stepDot' + i);
    var line = document.getElementById('stepLine' + i);
    if (dot) {
      dot.classList.remove('active', 'completed');
      if (i < step) dot.classList.add('completed');
      else if (i === step) dot.classList.add('active');
    }
    if (line) {
      line.classList.remove('completed');
      if (i < step) line.classList.add('completed');
    }
  }

  var prevBtn = document.getElementById('prevBtn');
  var nextBtn = document.getElementById('nextBtn');
  var submitBtn = document.getElementById('submitBtn');

  if (prevBtn) prevBtn.style.display = step > 1 ? 'inline-block' : 'none';
  if (nextBtn) nextBtn.style.display = step < totalSteps ? 'inline-block' : 'none';
  if (submitBtn) submitBtn.style.display = step === totalSteps ? 'inline-block' : 'none';
}

function nextStep() {
  if (currentStep < totalSteps) {
    if (!validateStep(currentStep)) return;
    showStep(currentStep + 1);
  }
}

function prevStep() {
  if (currentStep > 1) {
    showStep(currentStep - 1);
  }
}

function validateStep(step) {
  if (step === 1) {
    if (!applicationData.typeId) {
      showAlert('يرجى اختيار نوع الخدمة', 'warning');
      return false;
    }
  }
  if (step === 2) {
    var fullName = document.getElementById('fullName');
    var nationalId = document.getElementById('nationalId');
    var phone = document.getElementById('phone');
    var gov = document.getElementById('governorate');

    if (!fullName || !fullName.value.trim()) {
      showAlert('يرجى إدخال الاسم بالكامل', 'warning');
      return false;
    }
    if (!nationalId || nationalId.value.trim().length !== 14) {
      showAlert('يرجى إدخال الرقم القومي (14 رقم)', 'warning');
      return false;
    }
    if (!phone || phone.value.trim().length < 11) {
      showAlert('يرجى إدخال رقم الموبايل الصحيح', 'warning');
      return false;
    }
    if (!gov || !gov.value) {
      showAlert('يرجى اختيار المحافظة', 'warning');
      return false;
    }

    applicationData.fullName = fullName.value;
    applicationData.nationalId = nationalId.value;
    applicationData.phone = phone.value;
    applicationData.governorate = gov.value;
  }
  return true;
}

// ===== SUBMIT APPLICATION =====
function submitApplication() {
  var termsCheck = document.getElementById('termsCheck');
  if (termsCheck && !termsCheck.checked) {
    showAlert('يرجى الموافقة على الشروط والأحكام', 'warning');
    return;
  }

  // Generate application number
  var appNumber = 'CRS-2025-' + String(Math.floor(Math.random() * 9000) + 1000);
  applicationData.appNumber = appNumber;

  // Set initial status based on workflow
  var workflow = serviceWorkflows[applicationData.serviceId];
  var initialStep = workflow ? workflow.steps[0].id : 'submitted';

  applicationData.status = initialStep;
  applicationData.date = new Date().toLocaleDateString('ar-EG');
  applicationData.submittedAt = new Date().toISOString();
  applicationData.timeline = [
    {
      step: initialStep,
      date: new Date().toLocaleString('ar-EG'),
      note: 'تم تقديم الطلب بنجاح'
    }
  ];
  applicationData.appointments = [];

  // Save to localStorage
  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');
  apps.push(applicationData);
  localStorage.setItem('crs_applications', JSON.stringify(apps));

  // Show success
  var formContainer = document.getElementById('applicationForm');
  if (formContainer) {
    formContainer.innerHTML =
      '<div class="text-center py-5">' +
      '  <div style="width:100px;height:100px;border-radius:50%;background:#e8f5e9;color:#1a7a3a;display:inline-flex;align-items:center;justify-content:center;font-size:3rem;margin-bottom:20px;">' +
      '    <i class="bi bi-check-circle-fill"></i>' +
      '  </div>' +
      '  <h3 style="font-weight:800;color:var(--gov-primary);margin-bottom:10px;">تم تقديم الطلب بنجاح</h3>' +
      '  <p style="color:var(--gov-text-light);font-size:1.05rem;margin-bottom:5px;">رقم الطلب: <strong style="color:var(--gov-secondary);font-size:1.2rem;">' + appNumber + '</strong></p>' +
      '  <p style="color:var(--gov-text-light);font-size:0.9rem;margin-bottom:5px;">الخدمة: ' + applicationData.serviceName + ' - ' + applicationData.typeName + '</p>' +
      '  <p style="color:var(--gov-text-light);font-size:0.9rem;margin-bottom:5px;">الحالة: <strong style="color:#1565c0;">تم التقديم - بانتظار المراجعة</strong></p>' +
      '  <div class="alert alert-gov-info mt-3 mb-4" style="max-width:600px;margin:15px auto 25px;">' +
      '    <i class="bi bi-info-circle ms-2"></i>' +
      '    سيتم مراجعة طلبك من قبل الموظف المختص. في حال طلب رخصة القيادة، سيتم تحديد موعد الكشف الطبي تلقائياً بعد مراجعة المستندات.' +
      '  </div>' +
      '  <div class="d-flex gap-3 justify-content-center flex-wrap">' +
      '    <a href="track.html?id=' + appNumber + '" class="btn btn-gov-gold"><i class="bi bi-search ms-2"></i>تتبع الطلب</a>' +
      '    <a href="dashboard.html" class="btn btn-gov-primary"><i class="bi bi-house ms-2"></i>لوحة التحكم</a>' +
      '  </div>' +
      '</div>';
  }
}

// ===== TRACK APPLICATION =====
function trackApplication() {
  var trackInput = document.getElementById('trackInput');
  if (!trackInput || !trackInput.value.trim()) {
    showAlert('يرجى إدخال رقم الطلب', 'warning');
    return;
  }

  var appNumber = trackInput.value.trim();
  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');
  var found = apps.find(function(a) { return a.appNumber === appNumber; });

  var resultDiv = document.getElementById('trackResult');

  if (!found) {
    if (resultDiv) {
      resultDiv.classList.add('show');
      resultDiv.innerHTML =
        '<div class="text-center mb-4">' +
        '  <i class="bi bi-exclamation-circle" style="font-size:3rem;color:var(--gov-red);"></i>' +
        '  <h5 class="mt-3" style="font-weight:700;">لم يتم العثور على الطلب</h5>' +
        '  <p style="color:rgba(255,255,255,0.6);">تأكد من رقم الطلب وحاول مرة أخرى</p>' +
        '</div>';
    }
    return;
  }

  var workflow = serviceWorkflows[found.serviceId] || serviceWorkflows.birth;
  var statusInfo = getStatusInfo(found.serviceId, found.status);
  var progress = getApplicationProgress(found.serviceId, found.status);

  var timelineHtml = '';
  workflow.steps.forEach(function(step) {
    var timelineEntry = found.timeline ? found.timeline.find(function(t) { return t.step === step.id; }) : null;
    var isCurrent = found.status === step.id;
    var isPast = false;

    if (found.timeline) {
      var stepIdx = workflow.steps.findIndex(function(s) { return s.id === step.id; });
      var currentIdx = workflow.steps.findIndex(function(s) { return s.id === found.status; });
      isPast = stepIdx < currentIdx;
    }

    var cls = isCurrent ? 'current' : (isPast ? 'completed' : '');
    var dateText = timelineEntry ? timelineEntry.date : (isPast || isCurrent ? '' : 'بانتظار المراجعة');
    var noteText = timelineEntry ? timelineEntry.note : '';

    // Show appointment if exists
    var appointmentHtml = '';
    if (found.appointments && step.type === 'admin_assign') {
      var apt = found.appointments.find(function(a) { return a.step === step.id; });
      if (apt) {
        appointmentHtml = '<div style="font-size:0.8rem;color:var(--gov-secondary);margin-top:4px;">' +
          '<i class="bi bi-calendar-check ms-1"></i> موعد: ' + apt.date + ' - ' + apt.time +
          (apt.location ? ' | <i class="bi bi-geo-alt ms-1"></i>' + apt.location : '') +
          '</div>';
      }
    }

    timelineHtml +=
      '<div class="timeline-item ' + cls + '">' +
      '  <h6>' + step.name + '</h6>' +
      '  <p>' + dateText + '</p>' +
      (noteText ? '<p style="font-size:0.75rem;color:rgba(255,255,255,0.5);">' + noteText + '</p>' : '') +
      appointmentHtml +
      '</div>';
  });

  if (resultDiv) {
    resultDiv.classList.add('show');
    resultDiv.innerHTML =
      '<div class="text-center mb-4">' +
      '  <i class="bi bi-folder2-open" style="font-size:3rem;color:var(--gov-secondary);"></i>' +
      '  <h5 class="mt-3" style="font-weight:700;">' + found.serviceName + '</h5>' +
      '</div>' +
      '<div class="row mb-4">' +
      '  <div class="col-md-6 mb-2"><strong>رقم الطلب:</strong> ' + found.appNumber + '</div>' +
      '  <div class="col-md-6 mb-2"><strong>نوع الطلب:</strong> ' + found.typeName + '</div>' +
      '  <div class="col-md-6 mb-2"><strong>تاريخ التقديم:</strong> ' + found.date + '</div>' +
      '  <div class="col-md-6 mb-2"><strong>الحالة:</strong> <span style="color:' + statusInfo.color + ';font-weight:700;">' + statusInfo.name + '</span></div>' +
      '  <div class="col-12 mb-2"><strong>الرسوم:</strong> <span style="color:var(--gov-secondary);font-weight:700;">' + found.price + '</span></div>' +
      '</div>' +
      (found.status === 'rejected' ? '<div class="alert alert-gov-danger mb-4"><i class="bi bi-x-circle ms-2"></i>تم رفض الطلب' + (found.rejectionReason ? ': ' + found.rejectionReason : '') + '</div>' : '') +
      '<h6 style="font-weight:700;margin-bottom:15px;">مراحل الطلب</h6>' +
      '<div class="track-timeline">' + timelineHtml + '</div>';
  }
}

// ===== POPULATE USER DASHBOARD =====
function initDashboard() {
  if (!requireLogin()) return;

  var user = getUser();
  var userName = document.getElementById('userName');
  var navUserName = document.getElementById('navUserName');

  if (userName) userName.textContent = user.name;
  if (navUserName) navUserName.textContent = user.name;

  // Load applications
  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');
  var userApps = apps.filter(function(a) {
    return a.nationalId === user.nationalId || a.email === user.email || true;
  });

  var appsContainer = document.getElementById('userApplications');
  if (appsContainer) {
    appsContainer.innerHTML = '';

    if (userApps.length === 0) {
      appsContainer.innerHTML =
        '<div class="text-center py-4">' +
        '  <i class="bi bi-inbox" style="font-size:3rem;color:var(--gov-text-light);"></i>' +
        '  <p style="color:var(--gov-text-light);margin-top:10px;">لا توجد طلبات بعد</p>' +
        '  <a href="apply.html" class="btn btn-gov-gold mt-2"><i class="bi bi-plus-circle ms-1"></i> تقديم طلب جديد</a>' +
        '</div>';
    } else {
      userApps.forEach(function(app) {
        var statusInfo = getStatusInfo(app.serviceId, app.status);
        var progress = getApplicationProgress(app.serviceId, app.status);

        appsContainer.innerHTML +=
          '<div class="app-item">' +
          '  <div class="row align-items-center">' +
          '    <div class="col-md-4 mb-2 mb-md-0">' +
          '      <div class="fw-bold" style="color:#0d2137;">' + app.serviceName + '</div>' +
          '      <div style="font-size:0.78rem;color:#6c757d;">' + app.appNumber + ' | ' + app.typeName + '</div>' +
          '    </div>' +
          '    <div class="col-md-3 mb-2 mb-md-0">' +
          '      <div class="d-flex align-items-center gap-2">' +
          '        <span style="width:10px;height:10px;border-radius:50%;background:' + statusInfo.color + ';display:inline-block;"></span>' +
          '        <span style="font-size:0.88rem;font-weight:600;color:' + statusInfo.color + ';">' + statusInfo.name + '</span>' +
          '      </div>' +
          '    </div>' +
          '    <div class="col-md-3 mb-2 mb-md-0">' +
          '      <div class="progress-bar-gov"><div class="progress-fill" style="width:' + progress + '%;background:' + statusInfo.color + ';"></div></div>' +
          '      <div style="font-size:0.72rem;color:#6c757d;margin-top:4px;">' + progress + '% مكتمل</div>' +
          '    </div>' +
          '    <div class="col-md-2 text-md-start">' +
          '      <div style="font-size:0.82rem;color:#6c757d;">' + app.date + '</div>' +
          '    </div>' +
          '  </div>' +
          '</div>';
      });
    }
  }

  // Update appointments section
  var appointmentsContainer = document.getElementById('upcomingAppointments');
  if (appointmentsContainer) {
    appointmentsContainer.innerHTML = '';
    var hasAppointments = false;

    userApps.forEach(function(app) {
      if (app.appointments && app.appointments.length > 0) {
        app.appointments.forEach(function(apt) {
          // Only show future appointments
          var stepInfo = getStatusInfo(app.serviceId, apt.step);
          appointmentsContainer.innerHTML +=
            '<div class="d-flex gap-3 mb-3 p-3" style="background:#f0f4f8;border-radius:12px;">' +
            '  <div class="text-center" style="min-width:60px;">' +
            '    <div style="background:var(--gov-primary);color:var(--gov-secondary);border-radius:10px;padding:8px 12px;">' +
            '      <div style="font-size:1.2rem;font-weight:800;">' + apt.date.split('/')[0] + '</div>' +
            '      <div style="font-size:0.7rem;font-weight:600;">' + apt.date.split('/')[1] + '</div>' +
            '    </div>' +
            '  </div>' +
            '  <div class="flex-grow-1">' +
            '    <div class="fw-bold" style="color:var(--gov-primary);font-size:0.95rem;">' + stepInfo.name + '</div>' +
            '    <div style="font-size:0.82rem;color:var(--gov-text-light);">' + app.serviceName + ' - الموعد الساعة ' + apt.time + '</div>' +
            (apt.location ? '    <div style="font-size:0.78rem;color:var(--gov-secondary);margin-top:4px;"><i class="bi bi-geo-alt ms-1"></i> ' + apt.location + '</div>' : '') +
            '  </div>' +
            '</div>';
          hasAppointments = true;
        });
      }
    });

    if (!hasAppointments) {
      appointmentsContainer.innerHTML =
        '<div class="text-center py-3">' +
        '  <i class="bi bi-calendar-x" style="font-size:2rem;color:var(--gov-text-light);"></i>' +
        '  <p style="color:var(--gov-text-light);font-size:0.9rem;margin-top:8px;">لا توجد مواعيد قادمة</p>' +
        '</div>';
    }
  }
}

// ===== ADMIN FUNCTIONS =====

// Initialize admin dashboard
function initAdminDashboard() {
  if (!requireAdmin()) return;

  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');

  // Update stats
  var todayApps = apps.filter(function(a) {
    var today = new Date().toLocaleDateString('ar-EG');
    return a.date === today;
  });

  var pendingReview = apps.filter(function(a) { return a.status === 'submitted' || a.status === 'doc_review'; });
  var issued = apps.filter(function(a) { return a.status === 'issued'; });
  var rejected = apps.filter(function(a) { return a.status === 'rejected'; });

  var todayStat = document.getElementById('statToday');
  var reviewStat = document.getElementById('statReview');
  var issuedStat = document.getElementById('statIssued');
  var rejectedStat = document.getElementById('statRejected');

  if (todayStat) todayStat.textContent = todayApps.length;
  if (reviewStat) reviewStat.textContent = pendingReview.length;
  if (issuedStat) issuedStat.textContent = issued.length;
  if (rejectedStat) rejectedStat.textContent = rejected.length;

  // Populate applications table
  populateAdminTable(apps);
}

function populateAdminTable(apps) {
  var tbody = document.getElementById('adminAppsBody');
  if (!tbody) return;

  tbody.innerHTML = '';

  if (apps.length === 0) {
    tbody.innerHTML = '<tr><td colspan="7" class="text-center py-4">لا توجد طلبات</td></tr>';
    return;
  }

  apps.forEach(function(app, idx) {
    var statusInfo = getStatusInfo(app.serviceId, app.status);
    var statusClass = app.status === 'issued' ? 'badge-gov-success' :
                     app.status === 'rejected' ? 'badge-gov-danger' :
                     (app.status === 'submitted' || app.status === 'doc_review') ? 'badge-gov-warning' : 'badge-gov-info';

    tbody.innerHTML +=
      '<tr>' +
      '  <td style="font-weight:600;">' + app.appNumber + '</td>' +
      '  <td>' + (app.fullName || '-') + '</td>' +
      '  <td>' + app.serviceName + '</td>' +
      '  <td>' + app.typeName + '</td>' +
      '  <td><span class="' + statusClass + '">' + statusInfo.name + '</span></td>' +
      '  <td>' + app.date + '</td>' +
      '  <td><button class="btn btn-sm btn-gov-primary" style="padding:4px 14px;font-size:0.8rem;border-radius:8px;" onclick="openReviewModal(' + idx + ')">مراجعة</button></td>' +
      '</tr>';
  });
}

// Open review modal for an application
function openReviewModal(appIndex) {
  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');
  var app = apps[appIndex];
  if (!app) return;

  var workflow = serviceWorkflows[app.serviceId];
  if (!workflow) return;

  var currentStepIdx = workflow.steps.findIndex(function(s) { return s.id === app.status; });
  var nextStep = currentStepIdx < workflow.steps.length - 1 ? workflow.steps[currentStepIdx + 1] : null;
  var statusInfo = getStatusInfo(app.serviceId, app.status);

  // Build modal content
  var modalContent = document.getElementById('reviewModalBody');
  if (modalContent) {
    var html =
      '<div class="mb-3">' +
      '  <div class="row g-3">' +
      '    <div class="col-md-6"><strong>رقم الطلب:</strong> ' + app.appNumber + '</div>' +
      '    <div class="col-md-6"><strong>الخدمة:</strong> ' + app.serviceName + '</div>' +
      '    <div class="col-md-6"><strong>نوع الطلب:</strong> ' + app.typeName + '</div>' +
      '    <div class="col-md-6"><strong>الرسوم:</strong> ' + app.price + '</div>' +
      '    <div class="col-md-6"><strong>الاسم:</strong> ' + (app.fullName || '-') + '</div>' +
      '    <div class="col-md-6"><strong>الرقم القومي:</strong> ' + (app.nationalId || '-') + '</div>' +
      '    <div class="col-md-6"><strong>الموبايل:</strong> ' + (app.phone || '-') + '</div>' +
      '    <div class="col-md-6"><strong>المحافظة:</strong> ' + (app.governorate || '-') + '</div>' +
      '    <div class="col-12"><strong>الحالة الحالية:</strong> <span style="color:' + statusInfo.color + ';font-weight:700;">' + statusInfo.name + '</span></div>' +
      '  </div>' +
      '</div>' +
      '<hr style="border-color:var(--gov-border);">' +
      '<h6 style="font-weight:700;color:var(--gov-primary);margin-bottom:15px;">' +
      '  <i class="bi bi-arrow-repeat ms-2"></i>تحديث حالة الطلب' +
      '</h6>';

    if (app.status === 'rejected') {
      html += '<div class="alert alert-gov-danger">تم رفض هذا الطلب بالفعل</div>';
    } else if (nextStep) {
      html +=
        '<div class="alert alert-gov-info mb-3">' +
        '  <i class="bi bi-info-circle ms-2"></i>الخطوة التالية: <strong>' + nextStep.name + '</strong>' +
        '</div>';

      // If next step requires admin to assign date (medical exam, tests)
      if (nextStep.type === 'admin_assign') {
        html +=
          '<div class="mb-3">' +
          '  <label class="form-label fw-bold">تحديد موعد ' + nextStep.name + '</label>' +
          '  <div class="row g-2">' +
          '    <div class="col-md-4">' +
          '      <input type="date" class="form-control" id="assignDate">' +
          '    </div>' +
          '    <div class="col-md-4">' +
          '      <select class="form-select" id="assignTime">' +
          '        <option value="">اختر الوقت</option>' +
          '        <option value="9:00 ص">9:00 ص</option>' +
          '        <option value="10:00 ص">10:00 ص</option>' +
          '        <option value="11:00 ص">11:00 ص</option>' +
          '        <option value="12:00 م">12:00 م</option>' +
          '        <option value="1:00 م">1:00 م</option>' +
          '        <option value="2:00 م">2:00 م</option>' +
          '        <option value="3:00 م">3:00 م</option>' +
          '      </select>' +
          '    </div>' +
          '    <div class="col-md-4">' +
          '      <input type="text" class="form-control" id="assignLocation" placeholder="المكان (مثال: مكتب المرور - المعادي)">' +
          '    </div>' +
          '  </div>' +
          '</div>';
      }

      html +=
        '<div class="d-flex gap-2 flex-wrap">' +
        '  <button class="btn btn-gov-gold" onclick="advanceApplication(' + appIndex + ')">' +
        '    <i class="bi bi-check-circle ms-1"></i>تقدم للخطوة التالية' +
        '  </button>' +
        '  <button class="btn btn-gov-primary" onclick="rejectApplication(' + appIndex + ')" style="background:#c41e3a;color:#fff;">' +
        '    <i class="bi bi-x-circle ms-1"></i>رفض الطلب' +
        '  </button>' +
        '</div>';

      // Rejection reason
      html +=
        '<div class="mt-3" id="rejectionSection" style="display:none;">' +
        '  <label class="form-label fw-bold">سبب الرفض</label>' +
        '  <textarea class="form-control" id="rejectionReason" rows="2" placeholder="أدخل سبب رفض الطلب..."></textarea>' +
        '  <button class="btn btn-gov-primary mt-2" style="background:#c41e3a;color:#fff;" onclick="confirmReject(' + appIndex + ')">' +
        '    <i class="bi bi-x-circle ms-1"></i>تأكيد الرفض' +
        '  </button>' +
        '</div>';
    } else {
      html += '<div class="alert alert-gov-success">تم إكمال جميع خطوات الطلب بنجاح</div>';
    }

    // Show timeline
    if (app.timeline && app.timeline.length > 0) {
      html += '<hr style="border-color:var(--gov-border);"><h6 style="font-weight:700;margin-bottom:10px;">سجل الطلب</h6>';
      app.timeline.forEach(function(entry) {
        var info = getStatusInfo(app.serviceId, entry.step);
        html += '<div style="font-size:0.85rem;padding:5px 0;border-bottom:1px solid var(--gov-light);">' +
          '<span style="color:' + info.color + ';font-weight:600;">' + info.name + '</span> - ' + entry.date +
          (entry.note ? ' <span style="color:var(--gov-text-light);">(' + entry.note + ')</span>' : '') +
          '</div>';
      });
    }

    modalContent.innerHTML = html;
  }

  // Show modal
  var modalEl = document.getElementById('reviewModal');
  if (modalEl) {
    var modal = new bootstrap.Modal(modalEl);
    modal.show();
  }
}

// Advance application to next step
function advanceApplication(appIndex) {
  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');
  var app = apps[appIndex];
  if (!app) return;

  var workflow = serviceWorkflows[app.serviceId];
  if (!workflow) return;

  var currentStepIdx = workflow.steps.findIndex(function(s) { return s.id === app.status; });
  var nextStep = currentStepIdx < workflow.steps.length - 1 ? workflow.steps[currentStepIdx + 1] : null;

  if (!nextStep) {
    showAlert('الطلب مكتمل بالفعل', 'info');
    return;
  }

  // Check if next step requires date assignment
  if (nextStep.type === 'admin_assign') {
    var assignDate = document.getElementById('assignDate');
    var assignTime = document.getElementById('assignTime');
    var assignLocation = document.getElementById('assignLocation');

    if (!assignDate || !assignDate.value) {
      showAlert('يرجى تحديد تاريخ الموعد', 'warning');
      return;
    }
    if (!assignTime || !assignTime.value) {
      showAlert('يرجى تحديد وقت الموعد', 'warning');
      return;
    }

    // Save appointment
    if (!app.appointments) app.appointments = [];
    app.appointments.push({
      step: nextStep.id,
      date: assignDate.value,
      time: assignTime.value,
      location: assignLocation ? assignLocation.value : ''
    });
  }

  // Update status
  app.status = nextStep.id;
  if (!app.timeline) app.timeline = [];
  app.timeline.push({
    step: nextStep.id,
    date: new Date().toLocaleString('ar-EG'),
    note: 'تم التحديث بواسطة الأدمن'
  });

  apps[appIndex] = app;
  localStorage.setItem('crs_applications', JSON.stringify(apps));

  // Close modal
  var modalEl = document.getElementById('reviewModal');
  if (modalEl) {
    var modal = bootstrap.Modal.getInstance(modalEl);
    if (modal) modal.hide();
  }

  showAlert('تم تحديث حالة الطلب بنجاح إلى: ' + nextStep.name, 'success');
  initAdminDashboard();
}

// Show rejection form
function rejectApplication(appIndex) {
  var section = document.getElementById('rejectionSection');
  if (section) section.style.display = 'block';
}

// Confirm rejection
function confirmReject(appIndex) {
  var reason = document.getElementById('rejectionReason');
  var reasonText = reason ? reason.value : 'غير محدد';

  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');
  var app = apps[appIndex];
  if (!app) return;

  app.status = 'rejected';
  app.rejectionReason = reasonText;
  if (!app.timeline) app.timeline = [];
  app.timeline.push({
    step: 'rejected',
    date: new Date().toLocaleString('ar-EG'),
    note: 'تم رفض الطلب: ' + reasonText
  });

  apps[appIndex] = app;
  localStorage.setItem('crs_applications', JSON.stringify(apps));

  // Close modal
  var modalEl = document.getElementById('reviewModal');
  if (modalEl) {
    var modal = bootstrap.Modal.getInstance(modalEl);
    if (modal) modal.hide();
  }

  showAlert('تم رفض الطلب', 'danger');
  initAdminDashboard();
}

// Filter admin applications
function filterAdminApps(filter) {
  var apps = JSON.parse(localStorage.getItem('crs_applications') || '[]');

  if (filter === 'pending') {
    apps = apps.filter(function(a) { return a.status === 'submitted' || a.status === 'doc_review'; });
  } else if (filter === 'review') {
    apps = apps.filter(function(a) {
      return a.status !== 'submitted' && a.status !== 'doc_review' && a.status !== 'issued' && a.status !== 'rejected';
    });
  } else if (filter === 'issued') {
    apps = apps.filter(function(a) { return a.status === 'issued'; });
  } else if (filter === 'rejected') {
    apps = apps.filter(function(a) { return a.status === 'rejected'; });
  }

  populateAdminTable(apps);
}
