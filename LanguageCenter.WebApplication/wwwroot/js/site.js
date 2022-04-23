const languagesAjax = function () {
    let courseUl = document.getElementById('course-container');

    fetch('https://localhost:7188/Language/all-languages', {
        method: 'GET',
        cache: 'default'
    })
        .then(response => response.json())
        .then(data => data.forEach(d => courseUl.appendChild(createElement(d.name))));
}

function createElement(name) {

    let li = document.createElement('li');

    li.classList.add('nav-item');
    li.classList.add('active');

    let a = document.createElement('a');

    a.classList.add('nav-link');
    a.classList.add('main-menu-color');
    a.href = `/Course/AllCoursesByLanguage?language=${name}`;

    let i = document.createElement('i');

    i.classList.add('fas');
    i.classList.add('fa-chalkboard-teacher');
    i.classList.add('nav-icon');

    let p = document.createElement('p');
    p.textContent = `Курсове на ${name}`

    a.appendChild(i);
    a.appendChild(p);

    li.appendChild(a);

    return li;
}

window.onload = languagesAjax();