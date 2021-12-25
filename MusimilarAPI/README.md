MusimilarApi

Neo4j settings:

CREATE CONSTRAINT ON(l:GenreNode) ASSERT l.name IS UNIQUE
CREATE CONSTRAINT ON(l:ArtistNode) ASSERT l.mongoId IS UNIQUE