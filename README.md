# RPG Turn-Based Prototype
**Pre-Interview Test**

Proyek ini adalah prototype *Turn-Based* dengan arsitektur (design pattern). 

---

## Cara Bermain (Controls)

**Overworld Phase**
- `W` `A` `S` `D` / `Arrow Keys` : Bergerak eksplorasi
- `Space` : Interaksi dengan NPC & Objek (Memicu Dialog / Cutscene)
- `Esc` : Pause Menu

**Battle Phase (Turn-Based)**
- `Mouse Click` : Navigasi antarmuka UI, memilih aksi pertarungan (Attack, Special, Item), serta memilih target Lawan.

---

## Design Patterns

- **FSM (State Machine):** Mengontrol transisi fase pertarungan (`PlayerTurn`, `EnemyTurn`, dll) secara terisolasi melalui `IBattleState`.
- **Service Locator / Singleton:** Mengelola akses sistem inti (`GameManager`, `AudioManager`) secara terpusat dan rapi tanpa bergantung pada singleton tradisional.
- **Object Pooling:** Meminimalkan Garbage Collection, dengan mendaur ulang objek yang sering muncul (Elemen UI / Target Button & VFX).
- **Event Bus / Observer:** Membuat komunikasi antar script yang terputus (Broadcasting), khususnya untuk pembaruan HUD (HP/MP).
- **Scriptable Objects:** Memisahkan seluruh konfigurasi statis (Data Karakter, Statistik, & Item) dari logika kode agar bersifat Data-Driven.

---

## Tools & Assets

- **Engine:** Unity 6.3 LTS (6000.3.10f1)
- **Plugin Narasi:** Fungus V3.13.8 
- **Aset Visual & Audio:** Open Source Assets