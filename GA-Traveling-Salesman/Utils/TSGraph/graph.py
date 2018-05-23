import matplotlib.pyplot as plt
import matplotlib.animation as animation
import sys
import json
import numpy as np

"""
What it expects:
	argv[1] = max X
	argv[2] = max Y
	argv[3] = [option] == single/gif
	argv[4+] = jsons

	The json is encoded in the following way:
		{ points: [{"x": <nb>, "y": <nb>, "i": <nb>}, ...] }
		where "i" is the point's index.
"""

fig = plt.figure()
ax = fig.add_subplot(111)
ax = plt.axes(xlim=(0, float(sys.argv[1])), ylim=(0, float(sys.argv[2])))
line, = ax.plot([], [], 'ro-')

def init_frame():
	line.set_data([], [])
	return line,

def get_salesman_points(arg):
	data = json.loads(arg)
	points = []
	for point in data['points']:
		p = {'x': float(point['x']), 'y': float(point['y']), 'i': point['i']}
		points.append(p)	

	return points

def get_plot_xy(points):
	x = [p['x'] for p in points]
	y = [p['y'] for p in points]

	x.append(points[0]['x'])
	y.append(points[0]['y'])

	return x, y

def draw_points_indices(points):
	for p in points:
		ax.annotate('(%s)' % (p['i']), xy=(p['x'], p['y']), textcoords='data')

def draw_salesman_graph(points):
	x, y = get_plot_xy(points)
	plt.plot(x, y, 'ro-')
	draw_points_indices(points)
	plt.show()

def animate(i):
	points = get_salesman_points(sys.argv[4 + i])
	x, y = get_plot_xy(points)
	line.set_data(x, y)
	return line,

def show_gif():
	frames = len(sys.argv) - 4
	anim = animation.FuncAnimation( \
		fig,
		animate,
		init_func=init_frame,
		frames=frames,
		interval=20,
		blit=True)
	# anim.save('random.gif', writer='imagemagick', fps=60)
	plt.show()

if sys.argv[3] == "gif":
	show_gif()
elif sys.argv[3] == "single":
	draw_salesman_graph(get_salesman_points(sys.argv[4]))
