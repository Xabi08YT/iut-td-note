# Rapport — Load test 500k

**Test exécuté** : `task load-500k` (load test, 500 000 films)

## 1. Capture Grafana

_Collez ici une capture d’écran du dashboard Grafana (http://localhost:3000/d/k6-load-testing/k6-load-testing) pendant ou après l’exécution du test._

<!-- Remplacer par votre capture, ex. : ![Capture load-500k](captures/load-500k.png) -->

![Capture load-1M](captures/Load%201M.png)

## 2. Observations

_Décrivez ce que vous constatez lors de l’exécution du test (débit, latence, erreurs, comportement du système, etc.)._

- Latence en hausse avec 195ms pour les plus lents
- Aucune erreurs par contre
- Latencie irrégulière, mais reste quand même acceptable
