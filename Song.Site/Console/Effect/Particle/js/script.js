// alca.tv

const TAU = Math.PI * 2;
const { sin, cos, sqrt, atan2, random } = Math;

const canvas = document.getElementById('canvas');
const ctx = canvas.getContext('2d');
let width, height;

const noise = new SimplexNoise();

let showNoise = false;

const noiseTimeMult = 0.00005;
const density = 0.001;
const forceLength = 0.2;
const noiseCycle = 200;

const particles = [];
const particleCount = 8000;
const minimumLife = 1000; // ms
const friction = 0.95;

const lineWidth = 1.5;

window.addEventListener('load', () => {
  resize();
  for (let i = 0; i < particleCount; i++) {
    let p = new Particle();
    particles.push(p);
  }
  requestAnimationFrame(draw);
});

window.addEventListener('resize', resize);

function resize() {
  width = canvas.width = window.innerWidth;
  height = canvas.height = window.innerHeight;
  ctx.beginPath();
  ctx.rect(0, 0, width, height);
  ctx.fillStyle = 'hsl(219, 53%, 45%)';
  ctx.fill();
}

function draw(e) {
  requestAnimationFrame(draw);
  // ctx.filter = 'blur(0.1px)';
  // for(let i = 0; i < 36; i++) {
  // 	let t = i / 6 * TAU;
  // 	let x = cos(t) * 0.1;
  // 	let y = sin(t) * 0.1;
  // 	ctx.drawImage(canvas, x, y);
  // }
  // ctx.filter = 'none';

  // let time = e * 0.001;
  // let noiseTime = 0;
  // let noiseTime = cos(time) * 0.08;

  ctx.beginPath();
  ctx.rect(0, 0, width, height);
  ctx.fillStyle = 'hsl(219, 53%, 45%, 0.1)';
  ctx.fill();
  let noiseTime = e * noiseTimeMult;

  if (showNoise) {
    let count = 75;
    let xSize = width / count;
    let ySize = height / count;
    for (let y = 0; y < count; y++) {
      for (let x = 0; x < count; x++) {
        let xPos = xSize * x;
        let yPos = ySize * y;
        ctx.beginPath();
        ctx.rect(xPos, yPos, xSize, ySize);
        let angle = noise.noise3D(xPos * density, yPos * density, noiseTime);
        ctx.fillStyle = `hsl(${(angle + 1.5) * noiseCycle % 2 * 180}, 100%, 25%)`;
        ctx.fill();
      }
    }
  }

  ctx.beginPath();
  particles.forEach(p => {
    if (e - p.created > minimumLife) {
      p.show();
    }
    let angle = noise.noise3D(
    p.pos.x * density,
    p.pos.y * density,
    noiseTime);

    // let force = new Vector(0.06, 0)
    let force = new Vector(forceLength, 0)
    // .rotate(TAU * angle);
    .rotate(((angle + 1.5) * noiseCycle % 2 - 1) * TAU);
    p.applyForce(force);
    p.update();
    p.edges(e);
  });
  // ctx.strokeStyle = 'hsla(0, 0%, 0%, 0.5)';
  // ctx.lineWidth = 8;
  // ctx.stroke();
  ctx.strokeStyle = 'hsla(0, 0%, 96%, 0.5)';
  ctx.lineWidth = lineWidth;
  ctx.stroke();
}

class Vector {
  constructor(x = 0, y = 0) {
    this.x = x;
    this.y = y;
  }
  set(x = this.x, y = this.y) {
    if (x instanceof Vector) {
      ({ x, y } = x);
    }
    this.x = x;
    this.y = y;
    return this;
  }
  copy() {
    return new Vector(this.x, this.y);
  }
  add(x = 0, y = x) {
    if (x instanceof Vector) {
      ({ x, y } = x);
    }
    this.x += x;
    this.y += y;
    return this;
  }
  mult(x = 1, y = x) {
    if (x instanceof Vector) {
      ({ x, y } = x);
    }
    this.x *= x;
    this.y *= y;
    return this;
  }
  magSq() {
    return this.x * this.x + this.y * this.y;
  }
  mag() {
    return sqrt(this.magSq());
  }
  heading() {
    return atan2(this.y, this.x);
  }
  rotate(angle) {
    let newHeading = this.heading() + angle;
    let mag = this.mag();
    return this.set(
    cos(newHeading),
    sin(newHeading)).
    mult(mag);
  }}


class Particle {
  constructor() {
    this.pos = new Vector();
    this.lastPos = this.pos.copy();
    this.acc = new Vector();
    this.vel = new Vector();
    this.created = 0;
    this.randomize();
  }
  // reset() {
  // 	this.pos.set(0, 0);
  // 	this.lastPos = this.pos.copy();
  // 	this.acc.set(0, 0);
  // 	this.vel.set(0, 0);
  // }
  applyForce(...args) {
    this.acc.add(...args);
  }
  randomize() {
    this.pos.set(
    random() * width,
    random() * height);

    this.lastPos = this.pos.copy();
  }
  update() {
    this.lastPos = this.pos.copy();
    this.pos.add(this.vel.add(this.acc).mult(friction));
    this.acc.set(0, 0);
  }
  edges(e) {
    let { pos } = this;
    // let changed = false;
    // if(pos.x < 0) {
    // 	pos.x += width;
    // 	changed = true;
    // }
    // else if(pos.y < 0) {
    // 	pos.y += height;
    // 	changed = true;
    // }
    // else if(pos.x > width) {
    // 	pos.x -= width;
    // 	changed = true;
    // }
    // else if(pos.y > height) {
    // 	pos.y -= height;
    // 	changed = true;
    // }
    let changed = pos.x < 0 || pos.y < 0 || pos.x > width || pos.y > height;
    if (changed) {
      // this.lastPos = pos.copy();
      this.randomize();
      this.created = e;
    }
  }
  show() {
    let size = 1;
    ctx.moveTo(this.lastPos.x + size, this.lastPos.y);
    ctx.lineTo(this.pos.x + size, this.pos.y);
    // ctx.ellipse(this.pos.x, this.pos.y, size, size, 0, 0, TAU);
  }}